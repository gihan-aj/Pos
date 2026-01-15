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
            if (request.CategoryId.HasValue && request.CategoryId.Value != Guid.Empty)
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
            //if (!string.IsNullOrWhiteSpace(request.Brand))
            //{
            //    query = query.Where(p => p.Brand == request.Brand);
            //}

            // Active
            if (request.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == request.IsActive.Value);
            }

            // Search 
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();
                if (string.Equals(request.SearchIn, "name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Name.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "description", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Description != null && p.Description.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "sku", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Sku != null && p.Sku.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "brand", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Brand != null && p.Brand.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "material", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Material != null && p.Material.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "tags", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Tags != null && p.Tags.Any(tag => tag.Contains(term)));
                }
                else
                {
                    query = query.Where(p => 
                        p.Name.Contains(term) 
                        || (p.Sku != null && p.Sku.Contains(term))
                        || p.Tags != null && p.Tags.Any(tag => tag.Contains(term)));
                }
                
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
                p.Category != null ? p.Category.NamePath : "Unknown",
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
