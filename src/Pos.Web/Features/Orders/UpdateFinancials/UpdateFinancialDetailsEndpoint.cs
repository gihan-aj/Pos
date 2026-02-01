using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.UpdateFinancials
{
    public static class UpdateFinancialDetailsEndpoint
    {
        public static void MapUpdateFinancialDetails(this RouteGroupBuilder group)
        {
            group.MapPut("/{orderId}/financials", async (
                Guid orderId,
                UpdateFinancialDetailsCommand command,
                ISender mediator,
                CancellationToken cancellationToken = default) =>
            {
                if (orderId != command.OrderId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("UpdateFinancialDetails")
            .WithSummary("Update financial details in a existing order")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(400);
        }
    }
}
