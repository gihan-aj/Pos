namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public record ProductVariantDto(
        Guid Id,
        string Size,
        string Color,
        string Sku,
        decimal? Price,
        decimal? Cost,
        int StockQuantity);
}
