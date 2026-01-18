using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeleteCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            bool hasChildren = await _dbContext.Categories
                .AnyAsync(c => c.ParentCategoryId == command.Id, cancellationToken);

            if (hasChildren)
                return Result.Failure(Error.Conflict("Category.HasChildren", "Cannot delete a category that has sub-categories."));

            var hasProducts = await _dbContext.Products.AnyAsync(p => p.CategoryId == command.Id, cancellationToken);
            if (hasProducts)
                return Result.Failure(Error.Conflict("Category.HasProducts", "Cannot delete a category that contains products."));

            var category = await _dbContext.Categories
                .FindAsync([command.Id], cancellationToken);
            if(category is null)
                return Result.Failure(Error.NotFound("Category.NotFound", "Category not found."));

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
