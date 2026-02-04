using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.MarkAsDelivered
{
    public class MarkAsDeliveredHandler : ICommandHandler<MarkAsDeliveredCommand>
    {
        private readonly AppDbContext _dbContext;

        public MarkAsDeliveredHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(MarkAsDeliveredCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(new Error("Order.NotFound", "Order not found.", ErrorType.NotFound));

            var result = order.MarkAsDelivered();

            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
