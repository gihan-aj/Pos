using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.GetCustomer
{
    public record GetCustomerQuery(Guid Id) : IQuery<GetCustomerResponse>;
}
