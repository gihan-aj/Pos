using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.VoidPayment
{
    public static class VoidPaymentEndpoint
    {
        public static void MapVoidPayment(this RouteGroupBuilder group)
        {
            group.MapPut("/{orderId}/payments/{paymentId}/void", async (Guid orderId, Guid paymentId, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new VoidPaymentCommand(orderId, paymentId);
                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("VoidPayment")
            .WithSummary("Void a payment in an order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
