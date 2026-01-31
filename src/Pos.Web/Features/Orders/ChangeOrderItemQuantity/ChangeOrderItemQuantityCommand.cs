using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.ChangeOrderItemQuantity
{
    public record ChangeOrderItemQuantityCommand(
        Guid OrderId,
        Guid OrderItemId,
        int Quantity) : ICommand;
}
