using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetProductVariantList
{
    public record GetProductVariantListQuery: PagedRequest, IQuery<PagedList<ProductVariantListItem>>
    {
        public string? SearchIn { get; init; }
        public bool? IsActive { get; init; }
    }
}
