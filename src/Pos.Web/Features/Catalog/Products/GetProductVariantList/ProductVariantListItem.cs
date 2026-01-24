namespace Pos.Web.Features.Catalog.Products.GetProductVariantList
{
    public record ProductVariantListItem(
        Guid CategoryId,
        Guid ProductId,
        Guid VariantId,
        string CategoryPath,
        string CategoryName,
        string ProductName,
        string VariantName,
        string Sku,
        string Color,
        string Size,
        decimal? SellingPrice,
        int CurrentStock);
}
