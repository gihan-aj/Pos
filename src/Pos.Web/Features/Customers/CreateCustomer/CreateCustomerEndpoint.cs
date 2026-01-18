using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.CreateCustomer
{
    public static class CreateCustomerEndpoint
    {
        public static void MapCreateCustomer(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateCustomerCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("CreateCustomer")
            .WithSummary("Creates a new customer")
            .Produces<Guid>(200)
            .ProducesProblem(400)
            .ProducesProblem(409);
        }
    }
}
