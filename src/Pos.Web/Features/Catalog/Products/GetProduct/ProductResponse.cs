using Pos.Web.Features.Catalog.Entities;

namespace Pos.Web.Features.Catalog.Products.GetProduct
{
    public record ProductResponse(
        Guid Id,
        string Name,
        string? Description,
        Guid CategoryId,
        string CategoryName,
        string Sku,
        string? Brand,
        string? Material,
        Gender? Gender,
        decimal BasePrice,
        List<string> Tags,
        bool IsActive,
        List<ProductVariantDto> Variants,
        List<ProductImageDto> Images
    );
}
