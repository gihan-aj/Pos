using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.ActivateProduct
{
    public class ActivateProductHandler : ICommandHandler<ActivateProductCommand>
    {
        private readonly AppDbContext _dbContext;

        public ActivateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(ActivateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            var isCategoryActive = await _dbContext.Categories
                .AnyAsync(p => p.Id == product.CategoryId && p.IsActive, cancellationToken);

            if (!isCategoryActive)
                return Result.Failure(Error.Conflict("Category.NotActive", "Cannot activate a product when the category is inactive."));

            product.Activate();

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
