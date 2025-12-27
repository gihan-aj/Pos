using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.UpdateCategory
{
    public class UpdateCategoryHandler
    {
        private readonly AppDbContext _dbContext;

        public UpdateCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

            if(category is null)
                return Result.Failure(Error.NotFound("Category.NotFound", "Category not found"));
            

            if(category.Name != command.Name)
            {
                bool other = await _dbContext.Categories
                    .AnyAsync(c => c.Name == command.Name && c.Name != category.Name, cancellationToken);
                if(other)
                    return Result.Failure(Error.NotFound("Category.HasDuplicate", "Category name already in use."));
            }

            category.UpdateDetails(command.Name, command.Description, category.IconUrl, category.Color);

            if(category.ParentCategoryId != command.ParentCategoryId)
            {
                Category? newParent = null;

                if (command.ParentCategoryId.HasValue)
                {
                    newParent = await _dbContext.Categories
                        .FirstOrDefaultAsync(c => c.Id == command.ParentCategoryId.Value, cancellationToken);

                    if (newParent == null)
                        return Result.Failure(Error.NotFound("Parent.NotFound", "New parent category not found"));
                }

                var decendents = await _dbContext.Categories
                    .Where(c => c.Path.StartsWith(category.Path) && c.Id != category.Id)
                    .ToListAsync();
                // NOTE: EF Core "Navigation Fix-up" will automatically populate the category.SubCategories

                var moveResult = category.ChangeParent(newParent);
                if (moveResult.IsFailure) return moveResult;
            }

            return Result.Success();
        }
    }
}
