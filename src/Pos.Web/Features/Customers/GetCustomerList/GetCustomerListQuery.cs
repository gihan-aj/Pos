using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.GetCustomerList
{
    public record GetCustomerListQuery : PagedRequest, IQuery<PagedList<CustomerListItem>>
    {
        public bool? IsActive { get; init; }
        public string? SearchIn { get; set; }
    }
}
