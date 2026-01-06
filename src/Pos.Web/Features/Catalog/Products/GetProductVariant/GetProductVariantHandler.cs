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
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product is null)
                return Result.Failure<ProductVariantResponse>(Error.NotFound("Product.NotFound", "Base Product not found."));

            var variant = await _dbContext.Set<ProductVariant>()
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == request.VariantId && v.ProductId == request.ProductId, cancellationToken);

            if(variant is null)
                return Result.Failure<ProductVariantResponse>(Error.NotFound("Variant.NotFound", "Variant not found."));

            var response = new ProductVariantResponse(
                variant.Id,
                product.Name,
                variant.Sku,
                variant.Size,
                variant.Color,
                product.BasePrice,
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
