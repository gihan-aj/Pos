using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.MarkAsDelivered
{
    public record MarkAsDeliveredCommand(Guid OrderId) : ICommand;
}
