namespace Pos.Web.Features.Catalog.Products.GetProductVariant
{
    public record ProductVariantResponse(
        Guid Id,
        string ProductName,
        string Sku,
        string Size,
        string Color,
        decimal BasePrice,
        decimal? Price, // Effective Price (Base or Override)
        decimal? Cost,
        int StockQuantity,
        bool IsActive,
        bool IsAvailable
    );
}
