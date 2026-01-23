using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.SearchCustomers
{
    public record SearchCustomersQuery(string SearchTerm) : IQuery<List<SearchCustomerResponse>>;
}
