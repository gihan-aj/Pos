using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.GetProduct
{
    public class GetProductHandler : IQueryHandler<GetProductQuery, ProductResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ProductResponse>> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if(product is null)
                return Result.Failure<ProductResponse>(Error.NotFound("Product.NotFound", "Product not found."));

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.CategoryId,
                product.Category?.Name ?? "Unknown",
                product.Category?.NamePath ?? "Unknown",
                product.Sku ?? "",
                product.Brand,
                product.Material,
                product.Gender,
                product.BasePrice,
                product.Tags,
                product.IsActive,
                product.Variants.Any(v => v.IsAvailable),
                product.Images.FirstOrDefault(v => v.IsPrimary)?.ImageUrl ?? null,
                product.Images.Where(i => !i.IsPrimary).Select(i => i.ImageUrl).ToList(),
                product.Variants.Select(v => new ProductVariantDto(
                    v.Id,
                    v.Sku,
                    v.Size,
                    v.Color,
                    v.Price ?? product.BasePrice,
                    v.Cost,
                    v.StockQuantity,
                    v.IsActive,
                    v.IsAvailable
                )).OrderBy(v => v.Size).ThenBy(v => v.Color).ToList(),
                product.Images.Select(i => new ProductImageDto(
                    i.Id,
                    i.ImageUrl,
                    i.IsPrimary,
                    i.DiaplayOrder
                )).OrderBy(i => i.DisplayOrder).ToList()
            );

            return response;
        }
    }
}
