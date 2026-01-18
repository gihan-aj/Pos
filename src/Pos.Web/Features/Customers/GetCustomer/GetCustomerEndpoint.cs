using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.GetCustomer
{
    public static class GetCustomerEndpoint
    {
        public static void MapGetCustomer(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (Guid id, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(new GetCustomerQuery(id), cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetCustomer")
            .WithSummary("Get a existing customer by id")
            .Produces<GetCustomerResponse>(200)
            .ProducesProblem(404);
        }
    }
}
