using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.GetInventory
{
    public static class GetInventoryListEndpoint
    {
        public static void MapGetInventory(this RouteGroupBuilder group)
        {
            // Map to /api/products/inventory
            group.MapGet("/inventory", async ([AsParameters] GetInventoryQuery query, ISender sender) =>
            {
                var result = await sender.Send(query);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .WithName("GetInventoryList")
            .WithSummary("Get flat list of all variants (Inventory View)")
            .Produces<PagedList<InventoryItemDto>>(200);
        }
    }
}
