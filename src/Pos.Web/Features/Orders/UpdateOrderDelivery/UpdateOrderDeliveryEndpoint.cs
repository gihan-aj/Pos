using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.UpdateOrderDelivery
{
    public static class UpdateOrderDeliveryEndpoint
    {
        public static void MapUpdateOrderDelivery(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}", async (Guid id, UpdateOrderDeliveryCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                if (id != command.Id) 
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("UpdateOrderDelivery")
            .WithSummary("Update a existing order delivery details")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
