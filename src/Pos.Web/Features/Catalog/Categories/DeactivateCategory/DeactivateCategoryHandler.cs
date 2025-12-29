using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.DeactivateCategory
{
    public partial class DeactivateCategoryHandler : ICommandHandler<DeactivateCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeactivateCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeactivateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);
            if (category is null) return Result.Failure(Error.NotFound("Category.NotFound", "Category not found."));

            var decendents = await _dbContext.Categories
                .Where(c => c.Path.StartsWith(category.Path) && c.Id != category.Id)
                .ToListAsync();

            category.Deactivate();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
