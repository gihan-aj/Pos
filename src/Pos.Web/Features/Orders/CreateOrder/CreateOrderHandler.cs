using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Orders.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.CreateOrder
{
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

            var variantIds = command.Items.Select(oi => oi.ProductVariantId).ToList();
            var variants = await _dbContext.ProductVariants
                .Include(v => v.Product)
                .Where(v => variantIds.Contains(v.Id))
                .ToListAsync(cancellationToken);

            foreach(var item in command.Items)
            {
                var variant = variants.FirstOrDefault(v => v.Id == item.ProductVariantId);
                if (variant is null)
                    return Result.Failure<Guid>(new Shared.Errors.Error("OrderItem.NotFound", "Order item is not found.", Shared.Errors.ErrorType.NotFound));


                // AFTER CONFIRMATION OF THE ORDER????
                //var adjustStockResult = variant.AdjustStock(-item.Quantity);
                //if (adjustStockResult.IsFailure)
                //    return Result.Failure<Guid>(adjustStockResult.Error);

                var addResult = order.AddItem(variant.Product!, variant, item.Quantity);
                if (addResult.IsFailure)
                    return Result.Failure<Guid>(addResult.Error);
            }

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
