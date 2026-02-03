using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.CompleteProcess
{
    public record CompleteProcessingOrderCommand(Guid OrderId) : ICommand;
}
