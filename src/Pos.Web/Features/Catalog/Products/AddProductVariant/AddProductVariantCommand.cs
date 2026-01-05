using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.AddProductVariant
{
    public record AddProductVariantCommand(
        Guid ProductId,
        string Size,
        string Color,
        decimal? Price,
        decimal? Cost,
        int StockQuantity,
        string? SkuOverride) : ICommand;
}
