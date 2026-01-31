using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.AddOrderItem
{
    public record AddOrderItemCommand(
        Guid OrderId,
        Guid ProductVariantId,
        int Quantity) : ICommand;
}
