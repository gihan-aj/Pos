using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.CancelOrder
{
    public record CancelOrderCommand(
        Guid OrderId,
        string Reason,
        bool ReturnToStock) : ICommand;
}
