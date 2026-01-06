using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.DeactivateProductVariant
{
    public record DeactivateProductVariantCommand(Guid ProductId, Guid VariantId): ICommand;
}
