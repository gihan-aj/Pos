using Pos.Web.Features.Customers.ActivateCustomer;
using Pos.Web.Features.Customers.CreateCustomer;
using Pos.Web.Features.Customers.DeactivateCustomer;
using Pos.Web.Features.Customers.GetCustomer;
using Pos.Web.Features.Customers.GetCustomerList;
using Pos.Web.Features.Customers.SearchCustomers;
using Pos.Web.Features.Customers.UpdateCustomer;

namespace Pos.Web.Features.Customers
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customers")
                .WithTags("Customers");

            group.MapCreateCustomer();
            group.MapUpdateCustomer();
            group.MapActivateCustomer();
            group.MapDeactivateCustomer();
            group.MapGetCustomer();
            group.MapGetCustomerList();
            group.MapSearchCustomers();
        }
    }
}
