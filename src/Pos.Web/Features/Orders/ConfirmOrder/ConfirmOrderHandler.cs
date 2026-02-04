using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand>
    {
        private readonly AppDbContext _dbContext;

        public ConfirmOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(ConfirmOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            var variantIds = order.OrderItems
                .Select(oi => oi.ProductVariantId)
                .ToHashSet();

            var currentInventory = await _dbContext.ProductVariants
                .Where(v => variantIds.Contains(v.Id))
                .ToListAsync(cancellationToken);

            var result = order.Confirm(currentInventory);

            if (result.IsFailure)
            {
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            // await _publisher.Publish(new OrderConfirmedEvent(order.Id));
            // Listeners could then:
            // - Send email to customer ("We are working on it!")
            // - Notify Warehouse ("Pack this!")

            return Result.Success();
        }
    }
}
