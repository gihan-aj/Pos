using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.UpdateCustomer
{
    public static class UpdateCustomerEndpoint
    {
        public static void MapUpdateCustomer(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}", async (Guid id, UpdateCustomerCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                if (id != command.Id) return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("UpdateCustomer")
            .WithSummary("Update a existing customer")
            .Produces(204)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
