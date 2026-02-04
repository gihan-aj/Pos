using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.MarkAsShipped
{
    public record MarkAsShippedCommand(Guid OrderId) : ICommand;
}
