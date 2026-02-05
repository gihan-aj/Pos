using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.CancelOrder
{
    public class CancelOrderHandler : ICommandHandler<CancelOrderCommand>
    {
        private readonly AppDbContext _dbContext;

        public CancelOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

            if (order is null)
                return Result.Failure(Error.NotFound("Order.NotFound", "Order not found."));

            var inventory = new List<ProductVariant>();

            if(order.Status >= Entities.OrderStatus.Confirmed && command.ReturnToStock)
            {
                var variantIds = order.OrderItems.Select(o => o.ProductVariantId).Distinct().ToList();
                inventory = await _dbContext.ProductVariants
                    .Where(v => variantIds.Contains(v.Id))
                    .ToListAsync(cancellationToken);
            }

            var result = order.Cancel(command.Reason, command.ReturnToStock, inventory);

            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
