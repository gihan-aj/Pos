using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.VoidPayment
{
    public record VoidPaymentCommand(Guid OrderId, Guid PaymentId) : ICommand;
}
