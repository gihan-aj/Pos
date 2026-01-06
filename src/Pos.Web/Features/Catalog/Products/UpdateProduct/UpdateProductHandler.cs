using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            if(product.CategoryId != command.CategoryId)
            {
                var categoryInfo = await _dbContext.Categories
                    .Where(c => c.Id == command.CategoryId)
                    .Select(c => new { c.Id, HasChildren = c.SubCategories.Any()})
                    .FirstOrDefaultAsync(cancellationToken);

                if (categoryInfo is null)
                    return Result.Failure(Error.NotFound("Category.NotFound", "Category not found."));

                if (categoryInfo.HasChildren)
                    return Result.Failure<Guid>(Error.Conflict("Category.NotLeaf", "Products can only be assigned to leaf categories (categories with no sub-categories)."));
            }

            product.UpdateDetails(
                command.Name,
                command.Description,
                command.CategoryId,
                command.Brand,
                command.Material,
                command.Gender,
                command.BasePrice,
                command.Tags ?? new List<string>()
            );

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
