using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.DeactivateCustomer
{
    public static class DeactivateCustomerEndpoint
    {
        public static void MapDeactivateCustomer(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/deactivate", async (Guid id, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(new DeactivateCustomerCommand(id), cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
                .WithName("DeactivateCustomer")
                .WithSummary("Change the status of a customer to inactive")
                .Produces(204)
                .ProducesProblem(404);
        }
    }
}
