using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.DeactivateProductVariant
{
    public class DeactivateProductVariantHandler : ICommandHandler<DeactivateProductVariantCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeactivateProductVariantHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeactivateProductVariantCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
               .Include(p => p.Images)
               .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            var result = product.DeactivateVarient(command.VariantId);
            if (result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
