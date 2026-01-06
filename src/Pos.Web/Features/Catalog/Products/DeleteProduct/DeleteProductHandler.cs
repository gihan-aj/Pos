using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.DeleteProduct
{
    public class DeleteProductHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeleteProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (product is null) return Result.Failure(Error.NotFound("Product.NotFound", "Product not found"));

            // historical orders exist (future check).

            if (product.Variants.Any())
            {
                return Result.Failure(Error.Conflict("Product.HasVariants", "Cannot delete product with existing variants. Remove variants first."));
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }
}
