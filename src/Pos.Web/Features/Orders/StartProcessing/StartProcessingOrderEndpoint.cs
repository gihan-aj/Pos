using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.StartProcessing
{
    public static class StartProcessingOrderEndpoint
    {
        public static void MapStartProcessingOrder(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/process", async (
                Guid id,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                var command = new StartProcessingOrderCommand(id);

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("StartProcessingOrder")
            .WithSummary("Start processing an order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
