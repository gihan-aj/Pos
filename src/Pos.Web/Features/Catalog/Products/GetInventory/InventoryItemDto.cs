namespace Pos.Web.Features.Catalog.Products.GetInventory
{
    public record InventoryItemDto(
        Guid VariantId,
        Guid ProductId,
        string ProductName,
        string VariantName,
        string Sku,
        int StockQuantity,
        bool IsAvailable,
        decimal Price
    );
}
