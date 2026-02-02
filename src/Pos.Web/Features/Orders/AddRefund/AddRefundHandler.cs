using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.AddRefund
{
    public class AddRefundHandler : ICommandHandler<AddRefundCommand, Guid>
    {
        private readonly AppDbContext _dbContext;

        public AddRefundHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Handle(AddRefundCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure<Guid>(Error.NotFound("Order.NotFound", "Order is not found."));

            var result = order.AddRefund(
                command.Amount,
                command.PaymentDate,
                command.PaymentMethod,
                command.Reason,
                command.TransactionId);
            if(result.IsFailure)
                return Result.Failure<Guid>(result.Error);

            var refund = result.Value;
            var entry = _dbContext.Entry(refund);

            if (entry.State == EntityState.Detached)
            {
                _dbContext.OrderPayments.Add(refund);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(refund.Id);
        }
    }
}
