using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.CancelOrder
{
    public static class CancelOrderEndpoint
    {
        public static void MapCancelOrder(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/cancel", async (
                Guid id,
                CancelOrderCommand command,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                if (id != command.OrderId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("CancelOrder")
            .WithSummary("Cancel an existing order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
