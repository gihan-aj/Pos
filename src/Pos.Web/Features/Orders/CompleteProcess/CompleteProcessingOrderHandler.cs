using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.CompleteProcess
{
    public class CompleteProcessingOrderHandler : ICommandHandler<CompleteProcessingOrderCommand>
    {
        private readonly AppDbContext _dbContext;

        public CompleteProcessingOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(CompleteProcessingOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(new Error("Order.NotFound", "Order not found.", ErrorType.NotFound));

            var result = order.MarkAsReadyToShip();

            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
