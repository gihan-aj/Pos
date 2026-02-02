using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.ConfirmOrder
{
    public static class ConfirmOrderEndpoint
    {
        public static void MapConfirmOrder(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/confirm", async (
                Guid id,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                var command = new ConfirmOrderCommand(id);

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("ConfirmOrder")
            .WithSummary("Confirm an existing order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
