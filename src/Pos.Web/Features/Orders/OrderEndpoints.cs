using Pos.Web.Features.Orders.AddOrderItem;
using Pos.Web.Features.Orders.ChangeOrderItemQuantity;
using Pos.Web.Features.Orders.CreateOrder;
using Pos.Web.Features.Orders.GetOrder;
using Pos.Web.Features.Orders.GetOrderList;
using Pos.Web.Features.Orders.RemoveOrderItem;
using Pos.Web.Features.Orders.UpdateFinancials;
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
            group.MapAddOrderItem();
            group.MapChangeOrderItemQuantity();
            group.MapRemoveOrderItem();
            group.MapUpdateFinancialDetails();
        }
    }
}
