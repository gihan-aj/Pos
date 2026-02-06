using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.RemoveOrderItem
{
    public class RemoveOrderItemHandler : ICommandHandler<RemoveOrderItemCommand>
    {
        private readonly AppDbContext _dbContext;

        public RemoveOrderItemHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(RemoveOrderItemCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            if (order.OrderItems.Count == 1)
                return Result.Failure(Error.Conflict("Order.OnlyItem", "You cannot have emtpty orders."));

            var result = order.RemoveOrderItem(command.OrderItemId);
            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
