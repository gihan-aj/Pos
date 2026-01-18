using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.CreateOrder
{
    public record CreateOrderCommand : ICommand<Guid>
    {
        public Guid CustomerId { get; init; }
        public List<OrderItemDto> Items { get; init; } = new();

        // Delivery Info
        public string? DeliveryAddress { get; init; }
        public string? DeliveryCity { get; init; }
        public string? DeliveryPostalCode { get; init; }
        public string? Notes { get; init; }
        public decimal ShippingFee { get; init; }
    }
}
