using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.MarkAsShipped
{
    public static class MarkAsShippedEndpoint
    {
        public static void MapMarkAsShipped(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/ship", async (
                Guid id,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                var command = new MarkAsShippedCommand(id);

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("MarkAsShipped")
            .WithSummary("Marks a packed order as shipped.")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
