using Pos.Web.Shared.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pos.Web.Features.Catalog.Products.ActivateProductVariant
{
    public record ActivateProductVariantCommand(Guid ProductId, Guid VariantId) : ICommand;
}
