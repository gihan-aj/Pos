using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetProductList
{
    public record GetProductsQuery : PagedRequest, IQuery<PagedList<ProductListItem>>
    {
        public Guid? CategoryId { get; init; }
        public bool IncludeSubCategories { get; init; } = true;
        public bool? IsActive { get; init; }
        public string? SearchIn { get; set; }
    }
}
