namespace Pos.Web.Features.Catalog.Products.GetProduct
{
    public record ProductVariantDto(
        Guid Id,
        string Sku,
        string Size,
        string Color,
        decimal Price, // Effective Price (Base or Override)
        decimal? Cost,
        int StockQuantity,
        bool IsActive,
        bool IsAvailable
    );
}
