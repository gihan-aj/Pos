using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.ActivateCategory
{
    public class ActivateCategoryHandler
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

            // Do not activate sub categories
            category.Activate();

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
