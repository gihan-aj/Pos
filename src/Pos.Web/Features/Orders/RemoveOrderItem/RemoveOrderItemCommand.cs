using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.RemoveOrderItem
{
    public record RemoveOrderItemCommand(
        Guid OrderId,
        Guid OrderItemId): ICommand;
}
