using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.ActivateCategory
{
    public class ActivateCategoryHandler : ICommandHandler<ActivateCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public ActivateCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(ActivateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);
            if (category is null) 
                return Result.Failure(Error.NotFound("Category.NotFound", "Category not found."));

            if(category.ParentCategoryId is not null)
            {
                var parentCategory = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == category.ParentCategoryId, cancellationToken);

                if (parentCategory is null)
                    return Result.Failure(Error.NotFound("Category.NotFound", "Parent category not found."));

                if (!parentCategory.IsActive)
                    return Result.Failure(Error.Conflict("Category.ParentNotActive", "Cannot activate a category when parent category is not active."));
            }

            // Do not activate sub categories
            category.Activate();

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
