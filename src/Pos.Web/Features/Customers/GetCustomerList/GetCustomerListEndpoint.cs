using MediatR;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.GetCustomerList
{
    public static class GetCustomerListEndpoint
    {
        public static void MapGetCustomerList(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([AsParameters] GetCustomerListQuery query, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return result.IsSuccess 
                ? Results.Ok(result.Value) 
                : result.ToProblemDetails();
            })
            .WithName("GetCustomerList")
            .WithSummary("Search and filter customers")
            .Produces<PagedList<CustomerListItem>>(200);
        }
    }
}
