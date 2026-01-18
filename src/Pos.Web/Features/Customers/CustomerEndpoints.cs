using Pos.Web.Features.Customers.CreateCustomer;

namespace Pos.Web.Features.Customers
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customers")
                .WithTags("Customers");

            group.MapCreateCustomer();
        }
    }
}
