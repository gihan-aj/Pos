using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.UpdateFinancials
{
    public class UpdateFinancialDetailsHandler : ICommandHandler<UpdateFinancialDetailsCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateFinancialDetailsHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateFinancialDetailsCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            var result = order.UpdateFinancialDetails(command.ShippingFee, command.TaxAmount, command.DiscountAmount);
            if (result.IsFailure)
            {
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
