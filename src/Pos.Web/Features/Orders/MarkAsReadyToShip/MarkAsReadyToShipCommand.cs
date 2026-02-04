using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.MarkAsReadyToShip
{
    public record MarkAsReadyToShipCommand(Guid OrderId) : ICommand;
}
