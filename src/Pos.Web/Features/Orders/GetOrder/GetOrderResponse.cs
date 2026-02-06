using Pos.Web.Features.Orders.Entities;

namespace Pos.Web.Features.Orders.GetOrder
{
    public record GetOrderResponse(
        Guid Id,
        string OrderNumber,
        Guid CustomerId,
        CustomerDto? Customer,
        DateTime OrderDate,
        OrderStatus Status,
        OrderPaymentStatus PaymentStatus,
        // Financials
        decimal SubTotal,
        decimal DiscountAmount,
        decimal TaxAmount,
        decimal ShippingFee,
        decimal TotalAmount,
        decimal AmountPaid,
        decimal AmountDue,

        // Shipping Info
        bool IsCashOnDelivery,
        string DeliveryAddress,
        string? DeliveryCity,
        string? DeliveryRegion,
        string? DeliveryCountry,
        string? DeliveryPostalCode,
        Guid? CourierId,
        string? CourierName,
        string? TrackingNumber,

        string? Notes,

        // Collections
        List<OrderItemDto> Items,
        List<OrderPaymentDto> Payments);
}
