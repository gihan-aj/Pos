using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.MarkAsShipped
{
    public class MarkAsShippedHandler : ICommandHandler<MarkAsShippedCommand>
    {
        private readonly AppDbContext _dbContext;

        public MarkAsShippedHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(MarkAsShippedCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(new Error("Order.NotFound", "Order not found.", ErrorType.NotFound));

            var result = order.MarkAsShipped();

            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
