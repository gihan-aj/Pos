using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.AddRefund
{
    public static class AddRefundEndpoint
    {
        public static void MapAddRefund(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/refund", async (Guid id, AddRefundCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                if (id != command.OrderId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(new { Id = result.Value })
                    : result.ToProblemDetails();
            })
            .WithName("AddRefund")
            .WithSummary("Adds a refund to an order")
            .Produces(200)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
