using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.AddOrderItem
{
    public static class AddOrderItemEndpoint
    {
        public static void MapAddOrderItem(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/items", async (Guid id, AddOrderItemCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                if (id != command.OrderId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("AddOrderItem")
            .WithSummary("Add a order item to a existing order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
