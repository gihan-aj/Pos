using Pos.Web.Features.Orders.CreateOrder;
using Pos.Web.Features.Orders.GetOrder;
using Pos.Web.Features.Orders.GetOrderList;
using Pos.Web.Features.Orders.UpdateOrderDelivery;

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
            group.MapGetOrder();
            group.MapUpdateOrderDelivery();
        }
    }
}
