using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.MarkAsReadyToShip
{
    public static class MarkAsReadyToShipEndpoint
    {
        public static void MapMarkAsReadyToShip(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/ready-to-ship", async (
                Guid id,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                var command = new MarkAsReadyToShipCommand(id);

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("MarkAsReadyToShip")
            .WithSummary("Marks a processing order as packed and ready for shipping.")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
