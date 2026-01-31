using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.RemoveOrderItem
{
    public static class RemoveOrderItemEndpoint
    {
        public static void MapRemoveOrderItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{orderId}/items/{orderItemId}", async (
                Guid orderId,
                Guid orderItemId,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                var command = new RemoveOrderItemCommand(orderId, orderItemId);
                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("RemoveOrderItem")
            .WithSummary("Remove an item from an order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
