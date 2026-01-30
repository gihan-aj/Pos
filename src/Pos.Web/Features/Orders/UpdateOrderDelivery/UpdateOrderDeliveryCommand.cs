using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.UpdateOrderDelivery
{
    public record UpdateOrderDeliveryCommand : ICommand
    {
        public Guid Id { get; init; }
        public Guid CourierId { get; init; }
        public string DeliveryAddress { get; init; } = string.Empty;
        public string? DeliveryCity { get; init; }
        public string? DeliveryRegion { get; init; }
        public string? DeliveryCountry { get; init; }
        public string? DeliveryPostalCode { get; init; }
        public string? TrackingNumber { get; init; }
        public string? Notes { get; init; }
    }
}
