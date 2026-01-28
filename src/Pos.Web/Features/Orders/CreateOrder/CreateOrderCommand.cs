using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.CreateOrder
{
    public record CreateOrderCommand : ICommand<Guid>
    {
        public Guid CustomerId { get; init; }
        public List<OrderItemDto> Items { get; init; } = new();
        public string DeliveryAddress { get; init; } = string.Empty;
        public string? DeliveryCity { get; init; }
        public string? DeliveryRegion { get; init; }
        public string? DeliveryCountry { get; init; }
        public string? DeliveryPostalCode { get; init; }
        public string? Notes { get; init; }
        public Guid? CourierId { get; init; }
        public decimal ShippingFee { get; init; }
        public decimal TaxAmount { get; init; }
        public decimal DiscountAmount { get; init; }
        public List<OrderPaymentDto> OrderPayments { get; init; } = new();
    }
}
