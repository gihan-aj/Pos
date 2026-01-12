namespace Pos.Web.Features.Catalog.Products.CreateProduct
{
    public record CreateProductVariantDto(
        string Size,
        string Color,
        string Sku,
        decimal? Price,
        decimal? Cost,
        int StockQuantity);
}
