using Pos.Web.Features.Orders.Entities;

namespace Pos.Web.Features.Orders.GetOrder
{
    public record GetOrderResponse(
        Guid Id,
        string OrderNumber,
        Guid CustomerId,
        DateTime OrderDate,
        OrderStatus Status,
        PaymentStatus PaymentStatus,
        string DeliveryAddress,
        string? DeliveryCity,
        string? DeliveryPostalCode);
}
