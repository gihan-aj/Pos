using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.UpdateProductVariant
{
    public class UpdateProductVariantHandler : ICommandHandler<UpdateProductVariantCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateProductVariantHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateProductVariantCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);
            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            var result = product.UpdateVariant(
                command.VariantId,
                command.Size,
                command.Color,
                command.Sku,
                command.Price,
                command.Cost,
                command.StockQuantity
            );

            if (result.IsFailure) return result;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
