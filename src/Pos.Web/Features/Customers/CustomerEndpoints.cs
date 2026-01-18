using Pos.Web.Features.Customers.CreateCustomer;
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
        }
    }
}
