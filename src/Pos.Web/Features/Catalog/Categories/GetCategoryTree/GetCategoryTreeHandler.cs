using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public class GetCategoryTreeHandler : IQueryHandler<GetCategoryTreeQuery, List<CategoryTreeItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetCategoryTreeHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<CategoryTreeItem>>> Handle(GetCategoryTreeQuery query, CancellationToken cancellationToken)
        {
            var dbQuery = _dbContext.Categories.AsNoTracking().AsQueryable();

            if (query.IsActive)
            {
                dbQuery = dbQuery.Where(c => c.IsActive);
            }

            Guid? searchRootId = null;
            if (query.RootId.HasValue)
            {
                var rootEntity = await _dbContext.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == query.RootId.Value && c.IsActive == query.IsActive, cancellationToken);
                if (rootEntity is null)
                    return Result.Failure<List<CategoryTreeItem>>(Error.NotFound("Category.NotFound", "Category not found"));

                // Fetch root + descendents
                dbQuery = dbQuery.Where(c => c.Path.StartsWith(rootEntity.Path));
                searchRootId = rootEntity.Id;
            }

            var categories = await dbQuery
                .OrderBy(c => c.Level)
                .ThenBy(c => c.DisplayOrder)
                .ToListAsync(cancellationToken);

            if(categories.Count == 0)
            {
                return Result.Success(new List<CategoryTreeItem>());
            }

            var lookUp = categories.ToDictionary(
                c => c.Id,
                c => new CategoryTreeItem(c.Id, c.Name, c.NamePath, c.IsActive, c.DisplayOrder, new List<CategoryTreeItem>()));

            var resultRoots = new List<CategoryTreeItem>();
            foreach(var cat in categories)
            {
                // It is a root result if:
                // 1. It has no parent (Global Root)
                // 2. OR its parent is NOT in the fetched list (meaning it is the 'RootId' from the parameter)
                if (lookUp.TryGetValue(cat.Id, out var item))
                {
                    bool isRootNode = cat.ParentCategoryId is null || !lookUp.ContainsKey(cat.ParentCategoryId.Value);
                    if (isRootNode)
                    {
                        resultRoots.Add(item);
                    }
                    else
                    {
                        // It has a parent in the list, add to parent's children
                        if (cat.ParentCategoryId.HasValue && lookUp.TryGetValue(cat.ParentCategoryId.Value, out var parentItem))
                        {
                            parentItem.Children.Add(item);
                        }
                    }
                }
            }

            return Result.Success(resultRoots);
        }
    }
}
