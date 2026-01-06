using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, Category>
    {
        private readonly AppDbContext _dbContext;

        public CreateCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Category>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            bool nameExists = await _dbContext.Categories
                .AnyAsync(c => c.Name == command.Name && c.ParentCategoryId == command.ParentCategoryId, cancellationToken);
            if (nameExists)
            {
                return Result.Failure<Category>(Error.Conflict("Name.Duplicate", $"A category named '{command.Name}' already exists under the same parent category."));
            }

            Category? parentCategory = null;
            if (command.ParentCategoryId.HasValue)
            {
                parentCategory = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == command.ParentCategoryId.Value, cancellationToken);

                if(parentCategory is null)
                {
                    return Result.Failure<Category>(Error.NotFound("Parent.NotFound", "Parent category not found."));
                }
            }

            var categoryResult = Category.Create(
                command.Name,
                command.Description,
                parentCategory,
                command.DisplayOrder,
                command.IconUrl,
                command.Color
            );

            if (categoryResult.IsFailure)
            {
                return categoryResult;
            }

            var category = categoryResult.Value;
            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(category);
        }
    }
}
