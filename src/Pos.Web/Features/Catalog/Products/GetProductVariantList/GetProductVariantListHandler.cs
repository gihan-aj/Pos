using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetProductVariantList
{
    public class GetProductVariantListHandler : IQueryHandler<GetProductVariantListQuery, PagedList<ProductVariantListItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetProductVariantListHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedList<ProductVariantListItem>>> Handle(GetProductVariantListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.ProductVariants
                .AsNoTracking()
                .Include(v => v.Product)
                .ThenInclude(p => p!.Category)
                .AsQueryable();

            if (request.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == request.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();

                if (string.Equals(request.SearchIn, "productName", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Product != null && v.Product.Name.Contains(term));
                }
                else if(string.Equals(request.SearchIn, "size", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Size.Contains(term));
                }
                else if(string.Equals(request.SearchIn, "color", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Color.Contains(term));
                }
                else if(string.Equals(request.SearchIn, "sku", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Sku.Contains(term));
                }
                else if(string.Equals(request.SearchIn, "tags", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Product != null && v.Product.Tags.Contains(term));
                }
                else if(string.Equals(request.SearchIn, "brand", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Product != null && v.Product.Brand != null && v.Product.Brand.Contains(term));
                }
                else if(string.Equals(request.SearchIn, "material", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(v => v.Product != null && v.Product.Material != null && v.Product.Material.Contains(term));
                }
                else
                {
                    query = query
                        .Where(v => (v.Product != null && v.Product.Name.Contains(term))
                            || v.Size.Contains(term)
                            || v.Color.Contains(term)
                            || v.Sku.Contains(term)
                        );
                }
            }

            bool isAsc = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);
            query = request.SortBy?.ToLower() switch
            {
                "productName" => isAsc ? query.OrderBy(v => v.Product!.Name) : query.OrderByDescending(v => v.Product!.Name),
                "price" => isAsc ? query.OrderBy(v => v.Price) : query.OrderByDescending(v => v.Price),
                "created" => isAsc ? query.OrderBy(v => v.CreatedOnUtc) : query.OrderByDescending(v => v.CreatedOnUtc),
                _ => query.OrderByDescending(p => p.CreatedOnUtc) // Default
            };

            var projectedQuery = query.Select(v => new ProductVariantListItem(
                v.Product!.Category!.Id,
                v.Product.Id,
                v.Id,
                v.Product.Category.NamePath,
                v.Product.Category.Name,
                v.Product.Name,
                $"{v.Color}-{v.Size}",
                v.Sku,
                v.Color,
                v.Size,
                v.Price.HasValue ? v.Price : v.Product.BasePrice,
                v.StockQuantity));

            var pagedList = await PagedList<ProductVariantListItem>.CreateAsync(projectedQuery, request.Page, request.PageSize, cancellationToken);

            return Result.Success(pagedList);
        }
    }
}
