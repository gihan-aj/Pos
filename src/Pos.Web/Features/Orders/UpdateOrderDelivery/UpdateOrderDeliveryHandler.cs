using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.UpdateOrderDelivery
{
    public class UpdateOrderDeliveryHandler : ICommandHandler<UpdateOrderDeliveryCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateOrderDeliveryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateOrderDeliveryCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == command.Id, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order is not found."));

            var updateResult = order.UpdateDeliveryInfo(
                command.CourierId,
                command.DeliveryAddress,
                command.DeliveryCity,
                command.DeliveryRegion,
                command.DeliveryCountry,
                command.DeliveryPostalCode,
                command.TrackingNumber,
                command.Notes);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
