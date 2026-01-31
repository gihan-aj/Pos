using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.AddOrderItem
{
    public class AddOrderItemHandler : ICommandHandler<AddOrderItemCommand>
    {
        private readonly AppDbContext _dbContext;

        public AddOrderItemHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            var variant = await _dbContext.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.Id == command.ProductVariantId && v.IsActive, cancellationToken);

            if (variant is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product variant not found."));

            // TODO: check Stock Levels (variant.StockQuantity)

            var result = order.AddItem(variant.Product!, variant, command.Quantity);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            var item = result.Value;
            var entry = _dbContext.Entry(item);
            if(entry.State == EntityState.Detached)
            {
                _dbContext.OrderItems.Add(item);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
