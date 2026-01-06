using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetProduct
{
    public record GetProductQuery(Guid Id) : IQuery<ProductResponse>;
}
