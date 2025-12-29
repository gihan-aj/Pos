using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public class GetCategoryTreeHandler : IQueryHandler<GetCategoryTreeQuery, CategoryTreeItem>
    {
        private readonly AppDbContext _dbContext;

        public GetCategoryTreeHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CategoryTreeItem>> Handle(GetCategoryTreeQuery query, CancellationToken cancellationToken)
        {
            var rootQuery = _dbContext.Categories.AsNoTracking().AsQueryable();
            if (query.OnlyActive)
            {
                rootQuery = rootQuery.Where(c => c.IsActive);
            }

            var root = await rootQuery
                .FirstOrDefaultAsync(c => c.Id == query.RootId, cancellationToken);

            if (root is null)
                return Result.Failure<CategoryTreeItem>(Error.NotFound("Category.NotFound", "Category not found"));

            var decendentsQuery = _dbContext.Categories.AsNoTracking()
                .Where(c => c.Path.StartsWith(root.Path) && c.Id != root.Id);

            if (query.OnlyActive)
                decendentsQuery = decendentsQuery.Where(c => c.IsActive);

            var decendents = await decendentsQuery
                .OrderBy(c => c.Level)
                .ThenBy(c => c.DisplayOrder)
                .ToListAsync(cancellationToken);

            // Reconstruct tree in memory (O(n))
            var lookup = decendents.ToDictionary(
                c => c.Id,
                c => new CategoryTreeItem(c.Id, c.Name, c.IsActive, new List<CategoryTreeItem>()));

            // Add root to look up temporarily to ease linking if needed, or handle separately
            var rootItem = new CategoryTreeItem(root.Id, root.Name, root.IsActive, new List<CategoryTreeItem>());

            foreach (var category in decendents)
            {
                var item = lookup[category.Id];

                if(category.ParentCategoryId == root.Id)
                {
                    rootItem.Children.Add(item);
                }
                else if(category.ParentCategoryId.HasValue && lookup.TryGetValue(category.ParentCategoryId.Value, out var parentItem))
                {
                    parentItem.Children.Add(item);
                }
                // Orphan data should not exist
            }

            return Result.Success(rootItem);
        }
    }
}
