using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetProductList
{
    public class GetProductsHandler : IQueryHandler<GetProductsQuery, PagedList<ProductListItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetProductsHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedList<ProductListItem>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Products.AsNoTracking().AsQueryable();

            // --- FILTERING ---

            // Category Filter (Smart Tree Search)
            if (request.CategoryId.HasValue)
            {
                if (request.IncludeSubCategories)
                {
                    var categoryPath = await _dbContext.Categories
                        .Where(c => c.Id == request.CategoryId.Value)
                        .Select(c => c.Path)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (categoryPath != null)
                    {
                        query = query.Where(p => p.Category!.Path.StartsWith(categoryPath));
                    }
                    else
                    {
                        // Returning empty 
                        return Result.Success(new PagedList<ProductListItem>(new List<ProductListItem>(), request.Page, request.PageSize, 0));
                    }
                }
                else
                {
                    // Strict equality
                    query = query.Where(p => p.CategoryId == request.CategoryId.Value);
                }
            }

            // Brand
            if (!string.IsNullOrWhiteSpace(request.Brand))
            {
                query = query.Where(p => p.Brand == request.Brand);
            }

            // Active
            if (request.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == request.IsActive.Value);
            }

            // Search (Name or SKU)
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();
                query = query.Where(p => p.Name.Contains(term) || (p.Sku != null && p.Sku.Contains(term)));
            }

            // --- SORTING ---
            bool isAsc = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);
            query = request.SortBy?.ToLower() switch
            {
                "name" => isAsc ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                "price" => isAsc ? query.OrderBy(p => p.BasePrice) : query.OrderByDescending(p => p.BasePrice),
                "created" => isAsc ? query.OrderBy(p => p.CreatedOnUtc) : query.OrderByDescending(p => p.CreatedOnUtc),
                _ => query.OrderByDescending(p => p.CreatedOnUtc) // Default
            };

            // --- PROJECTION ---
            var projectedQuery = query.Select(p => new ProductListItem(
                p.Id,
                p.Name,
                p.Category != null ? p.Category.Name : "Unknown",
                p.Sku,
                p.BasePrice,
                p.Variants.Count,
                p.Variants.Sum(v => v.StockQuantity), // Aggregated Stock
                p.Images.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault(),
                p.IsActive
            ));

            // --- EXECUTION ---
            var pagedList = await PagedList<ProductListItem>.CreateAsync(projectedQuery, request.Page, request.PageSize, cancellationToken);

            return Result.Success(pagedList);
        }
    }
}
