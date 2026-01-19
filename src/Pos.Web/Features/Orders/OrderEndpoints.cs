using Pos.Web.Features.Orders.CreateOrder;
using Pos.Web.Features.Orders.GetOrderList;

namespace Pos.Web.Features.Orders
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/orders")
                .WithTags("Orders");

            group.MapCreateOrder();
            group.MapGetOrderList();
        }
    }
}
