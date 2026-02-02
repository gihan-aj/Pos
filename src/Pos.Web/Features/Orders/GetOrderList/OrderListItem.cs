using Pos.Web.Features.Orders.Entities;

namespace Pos.Web.Features.Orders.GetOrderList
{
    public record OrderListItem(
        Guid Id,
        string OrderNumber,
        string CustomerName,
        DateTime OrderDate,
        OrderStatus Status,
        OrderPaymentStatus PaymentStatus,
        decimal TotalAmount,
        int ItemCount,
        string? PaymentMethod,
        string? DeliveryCity);
}
