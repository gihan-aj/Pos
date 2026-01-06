using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.GetProductVariant
{
    public class GetProductVariantHandler : IQueryHandler<GetProductVariantQuery, ProductVariantResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetProductVariantHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ProductVariantResponse>> Handle(GetProductVariantQuery request, CancellationToken cancellationToken)
        {
            //var product = await _dbContext.Products
            //    .AsNoTracking()
            //    .Include(p => p.Variants)
            //    .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            //if (product is null)
            //    return Result.Failure<ProductVariantResponse>(Error.NotFound("Product.NotFound", "Base Product not found."));

            //var variant = product.Variants
            //    .FirstOrDefault(v => v.Id == request.VariantId);

            var data = await _dbContext.Products
                .AsNoTracking()
                .Where(p => p.Id == request.ProductId)
                .SelectMany(p => p.Variants, (p, v) => new { p, v })
                .Where(x => x.v.Id == request.VariantId)
                .Select(x => new
                {
                    Variant = x.v,
                    ProductName = x.p.Name,
                    BasePrice = x.p.BasePrice
                })
                .FirstOrDefaultAsync(cancellationToken);

            if(data is null)
                return Result.Failure<ProductVariantResponse>(Error.NotFound("Variant.NotFound", "Variant not found."));

            var variant = data.Variant;

            var response = new ProductVariantResponse(
                variant.Id,
                data.ProductName,
                variant.Sku,
                variant.Size,
                variant.Color,
                data.BasePrice,
                variant.Price,
                variant.Cost,
                variant.StockQuantity,
                variant.IsActive,
                variant.IsAvailable
            );

            return response;
        }
    }
}
