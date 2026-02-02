using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.StartProcessing
{
    public record StartProcessingOrderCommand(Guid OrderId): ICommand;
}
