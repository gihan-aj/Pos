using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.ActivateCustomer
{
    public static class ActivateCustomerEndpoint
    {
        public static void MapActivateCustomer(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/activate", async (Guid id, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(new ActivateCustomerCommand(id), cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
                .WithName("ActivateCustomer")
                .WithSummary("Change the status of a customer to active")
                .Produces(204)
                .ProducesProblem(404);
        }
    }
}
