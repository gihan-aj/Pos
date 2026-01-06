using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetProductVariant
{
    public record GetProductVariantQuery(Guid ProductId, Guid VariantId): IQuery<ProductVariantResponse>;
}
