using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Enums;

namespace Pos.Web.Features.Orders.AddPayment
{
    public record AddPaymentCommand(
        Guid OrderId,
        decimal Amount,
        DateTime PaymentDate,
        PaymentMethod PaymentMethod,
        string? TransactionId,
        string? Notes) : ICommand<Guid>;
}
