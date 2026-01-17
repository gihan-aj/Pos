namespace Pos.Web.Features.Catalog.Products.GetProductList
{
    public record ProductListItem(
        Guid Id,
        string Name,
        string CategoryName,
        string? Sku,
        decimal BasePrice,
        int ActiveVariantCount,
        int VariantCount,
        int ActiveStock,
        int TotalStock,
        string? PrimaryImageUrl,
        bool IsActive
    );
}
