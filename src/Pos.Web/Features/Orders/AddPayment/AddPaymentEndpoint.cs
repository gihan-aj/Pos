using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.AddPayment
{
    public static class AddPaymentEndpoint
    {
        public static void MapAddPayment(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/payments", async (Guid id, AddPaymentCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                if (id != command.OrderId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(new { Id = result.Value })
                    : result.ToProblemDetails();
            })
            .WithName("AddPayment")
            .WithSummary("Adds a new payment to an order")
            .Produces(200)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
