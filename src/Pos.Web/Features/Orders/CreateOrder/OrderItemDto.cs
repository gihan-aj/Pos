using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Orders.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.CreateOrder
{
    public record OrderItemDto(Guid ProductId, Guid ProductVariantId, int Quantity);

    public record CreateOrderCommand : ICommand<Guid>
    {
        public Guid CustomerId { get; init; }
        public List<OrderItemDto> Items { get; init; } = new();

        // Delivery Info
        public string? DeliveryAddress { get; init; }
        public string? DeliveryCity { get; init; }
        public string? DeliveryPostalCode { get; init; }
        public string? Notes { get; init; }
        public decimal ShippingFee { get; init; }
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer Id is required.");
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one order item is required.");
            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductVariantId)
                    .NotEmpty().WithMessage("Product Variant Id is required.");
                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            });
        }
    }

    public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, Guid>
    {
        private readonly AppDbContext _dbContext;

        public CreateOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            bool customerExists = await _dbContext.Customers
                .AnyAsync(c => c.Id == command.CustomerId && c.IsActive);
            if (!customerExists)
                return Result.Failure<Guid>(new Shared.Errors.Error("Order.CustomerNotFound", "Customer not found.", Shared.Errors.ErrorType.NotFound));

            // Temporary !!!!!
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

            var orderResult = Order.Create(
                command.CustomerId,
                orderNumber,
                command.DeliveryAddress,
                command.DeliveryCity,
                command.DeliveryPostalCode,
                command.Notes,
                command.ShippingFee);

            if (orderResult.IsFailure)
                return Result.Failure<Guid>(orderResult.Error);

            var order = orderResult.Value;

            var productIds = command.Items.Select(oi => oi.ProductId).Distinct();
            var variantIds = command.Items.Select(oi => oi.ProductVariantId).ToList();
            return Guid.NewGuid();
        }
    }
}
