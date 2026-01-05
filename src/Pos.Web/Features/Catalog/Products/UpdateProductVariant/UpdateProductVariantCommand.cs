using Pos.Web.Shared.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pos.Web.Features.Catalog.Products.UpdateProductVariant
{
    public record UpdateProductVariantCommand(
        Guid ProductId,
        Guid VariantId,
        string Size,
        string Color,
        string Sku,
        decimal? Price,
        decimal? Cost,
        int StockQuantity) : ICommand;
}
