using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.MarkAsDelivered
{
    public static class MarkAsDeliveredEndpoint
    {
        public static void MapMarkAsDelivered(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/deliver", async (
                Guid id,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                var command = new MarkAsDeliveredCommand(id);

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("MarkAsDelivered")
            .WithSummary("Marks a order as delivered.")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
