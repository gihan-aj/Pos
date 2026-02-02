using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.VoidPayment
{
    public class VoidPaymentHandler : ICommandHandler<VoidPaymentCommand>
    {
        private readonly AppDbContext _dbContext;

        public VoidPaymentHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(VoidPaymentCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            var result = order.VoidPayment(command.PaymentId);
            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
