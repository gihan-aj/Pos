using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public class CreateCategoryHandler
    {
        private readonly AppDbContext _dbContext;

        public CreateCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Category>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            bool nameExists = await _dbContext.Categories
                .AnyAsync(c => c.Name == command.Name, cancellationToken);
            if (nameExists)
            {
                return Result.Failure<Category>(Error.Conflict("Name.Duplicate", $"The category '{command.Name}' already exists."));
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

            // --- THE MAGIC ---
            // We DO NOT call SaveChangesAsync() here.
            // Wolverine's "Transactional Middleware" (configured in Program.cs) detects this handler
            // uses EF Core and will automatically:
            //   1. Open a Transaction
            //   2. Run this Handle method
            //   3. Call SaveChangesAsync
            //   4. Commit Transaction
            //   5. (Bonus) Dispatch any Domain Events raised by the entity

            return Result.Success(category);
        }
    }
}
