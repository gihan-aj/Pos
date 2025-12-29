using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryList
{
    public class GetCategoryListHandler : IQueryHandler<GetCategoriesQuery, PagedList<CategoryListItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetCategoryListHandler(AppDbContext dbCOntext)
        {
            _dbContext = dbCOntext;
        }

        public async Task<Result<PagedList<CategoryListItem>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Categories.AsNoTracking().AsQueryable();

            // FILTERS
            if (request.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == request.IsActive.Value);
            }

            if (request.ParentCategoryId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == request.ParentCategoryId.Value);
            }

            if(request.Ids is not null && request.Ids.Length > 0)
            {
                query = query.Where(c => request.Ids.Contains(c.Id));
            }

            if (request.Level.HasValue)
            {
                query = query.Where(c => c.Level == request.Level.Value);
            }

            // SEARCH
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();

                if(string.Equals(request.SearchIn, "name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Name.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "description", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Description != null && c.Description.Contains(term));
                }
                else
                {
                    // Default: Search both
                    query = query.Where(c => c.Name.Contains(term) || (c.Description != null && c.Description.Contains(term)));
                }
            }

            // HAS PRODUCTS FILTER
            //if (request.HasProducts.HasValue)
            //{
            //    if (request.HasProducts.Value)
            //    {
            //        query = query.Where(c => _dbContext.Set<Product>().Any(p => p.CategoryId == c.Id));
            //    }
            //    else
            //    {
            //        query = query.Where(c => !_dbContext.Set<Product>().Any(p => p.CategoryId == c.Id));
            //    }
            //}

            // SORT
            bool isAsc = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            query = request.SortBy?.ToLower() switch
            {
                "name" => isAsc ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                "createdat" => isAsc ? query.OrderBy(c => c.CreatedOnUtc) : query.OrderByDescending(c => c.CreatedOnUtc),
                "displayorder" => isAsc ? query.OrderBy(c => c.DisplayOrder) : query.OrderByDescending(c => c.DisplayOrder),
                // Complex Sort: Product Count
                //"productcount" => isAsc
                //    ? query.OrderBy(c => _dbContext.Set<Product>().Count(p => p.CategoryId == c.Id))
                //    : query.OrderByDescending(c => _dbContext.Set<Product>().Count(p => p.CategoryId == c.Id)),
                // Default Sort
                _ => query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
            };

            // PROJECTION & PAGING
            var projectedQuery = query.Select(c => new CategoryListItem(
                c.Id,
                c.Name,
                c.Description,
                c.IsActive,
                c.Level,
                c.DisplayOrder,
                c.ParentCategoryId,
                null
                // Subquery for count
                //_dbContext.Set<Product>().Count(p => p.CategoryId == c.Id)
            ));

            var pagedResult = await PagedList<CategoryListItem>.CreateAsync(
                projectedQuery,
                request.Page,
                request.PageSize,
                cancellationToken);

            return Result.Success(pagedResult);
        }
    }
}
