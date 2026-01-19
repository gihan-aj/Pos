using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.GetOrderList
{
    public static class GetOrderListEndpoint
    {
        public static void MapGetOrderList(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([AsParameters] GetOrderListQuery query, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails();
            })
            .WithName("GetOrderList")
            .WithSummary("Search and filter orders")
            .Produces<PagedList<OrderListItem>>(200);
        }
    }
}
