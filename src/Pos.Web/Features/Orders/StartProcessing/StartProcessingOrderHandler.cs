using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.StartProcessing
{
    public class StartProcessingOrderHandler : ICommandHandler<StartProcessingOrderCommand>
    {
        private readonly AppDbContext _dbContext;

        public StartProcessingOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(StartProcessingOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            var result = order.StartProcessing();

            if (result.IsFailure)
            {
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
