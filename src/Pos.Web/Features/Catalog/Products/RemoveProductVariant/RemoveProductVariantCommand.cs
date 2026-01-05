using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.RemoveProductVariant
{
    public record RemoveProductVariantCommand(Guid ProductId, Guid VariantId): ICommand;
}
