using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.ChangeOrderItemQuantity
{
    public static class ChangeOrderItemQuantityEndpoint
    {
        public static void MapChangeOrderItemQuantity(this RouteGroupBuilder group)
        {
            group.MapPut("/{orderId}/items/{orderItemId}/quantity", async (
                Guid orderId, 
                Guid orderItemId, 
                ChangeOrderItemQuantityCommand command, 
                ISender mediator, 
                CancellationToken cancellationToken = default) =>
            {
                if (orderId != command.OrderId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                if (orderItemId != command.OrderItemId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("ChangeOrderItemQuantity")
            .WithSummary("Change the quantity of a order item in a existing order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
