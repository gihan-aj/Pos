using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetInventory
{
    public class GetInventoryHandler : IQueryHandler<GetInventoryQuery, PagedList<InventoryItemDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetInventoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedList<InventoryItemDto>>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
        {
            // Join product and variant
            //var query = from v in _dbContext.Set<ProductVariant>().AsNoTracking()
            //            join p in _dbContext.Products.AsNoTracking() on v.ProductId equals p.Id
            //            select new { v, p };

            // This generates: SELECT ... FROM Products p INNER JOIN ProductVariants v ON p.Id = v.ProductId
            var query = _dbContext.Products
                .AsNoTracking()
                .SelectMany(p => p.Variants, (p, v) => new { p, v });

            // --- FILTERING ---
            if (request.ProductId.HasValue)
            {
                query = query.Where(x => x.v.ProductId == request.ProductId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Sku))
            {
                query = query.Where(x => x.v.Sku.Contains(request.Sku));
            }

            if(request.LowStock.HasValue && request.LowStock.Value)
            {
                // Hard coded or configurable??
                query = query.Where(x => x.v.StockQuantity <= 5);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();
                query = query.Where(x => x.p.Name.Contains(term) || x.v.Sku.Contains(term));
            }

            // --- SORTING ---
            bool isAsc = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);
            query = request.SortBy?.ToLower() switch
            {
                "sku" => isAsc ? query.OrderBy(x => x.v.Sku) : query.OrderByDescending(x => x.v.Sku),
                "stock" => isAsc ? query.OrderBy(x => x.v.StockQuantity) : query.OrderByDescending(x => x.v.StockQuantity),
                "product" => isAsc ? query.OrderBy(x => x.p.Name) : query.OrderByDescending(x => x.p.Name),
                _ => query.OrderBy(x => x.v.StockQuantity) // Default: Show low stock first? Or Sku?
            };

            // --- PROJECTION ---
            var projectedQuery = query.Select(x => new InventoryItemDto(
                x.v.Id,
                x.p.Id,
                x.p.Name,
                $"{x.v.Color} / {x.v.Size}",
                x.v.Sku,
                x.v.StockQuantity,
                x.v.IsAvailable,
                x.v.Price ?? x.p.BasePrice
             ));

            // --- EXECUTION ---
            var pagedList = await PagedList<InventoryItemDto>.CreateAsync(projectedQuery, request.Page, request.PageSize, cancellationToken);

            return Result.Success(pagedList);
        }
    }
}
