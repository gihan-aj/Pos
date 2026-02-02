using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.ConfirmOrder
{
    public record ConfirmOrderCommand(Guid OrderId) : ICommand;
}
