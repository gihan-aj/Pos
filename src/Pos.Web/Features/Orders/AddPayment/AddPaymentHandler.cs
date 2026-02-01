using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.AddPayment
{
    public class AddPaymentHandler : ICommandHandler<AddPaymentCommand, Guid>
    {
        private readonly AppDbContext _dbContext;

        public AddPaymentHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Handle(AddPaymentCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure<Guid>(Error.NotFound("Order.NotFound", "Order is not found."));

            var result = order.AddPayment(
                command.Amount,
                command.PaymentDate,
                command.PaymentMethod,
                command.TransactionId,
                command.Notes);

            if (result.IsFailure)
                return Result.Failure<Guid>(result.Error);

            var payment = result.Value;
            var entry = _dbContext.Entry(payment);

            if (entry.State == EntityState.Detached)
            {
                _dbContext.OrderPayments.Add(payment);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(payment.Id);
        }
    }
}
