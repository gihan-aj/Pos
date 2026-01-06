using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.GetInventory
{
    public record GetInventoryQuery : PagedRequest , IQuery<PagedList<InventoryItemDto>>
    {
        public bool? LowStock { get; init; }
        public string? Sku { get; init; }
        public Guid? ProductId { get; init; }
    }
}
