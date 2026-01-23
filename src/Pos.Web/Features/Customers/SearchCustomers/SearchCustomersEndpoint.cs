using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Customers.SearchCustomers
{
    public static class SearchCustomersEndpoint
    {
        public static void MapSearchCustomers(this RouteGroupBuilder group)
        {
            group.MapGet("/search", async (string searchTerm, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new SearchCustomersQuery(searchTerm), cancellationToken);
                return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails();
            })
            .WithName("SearchCustomers")
            .WithSummary("Search customers by name, phone, email or country")
            .Produces<List<SearchCustomerResponse>>(200);
        }
    }
}
