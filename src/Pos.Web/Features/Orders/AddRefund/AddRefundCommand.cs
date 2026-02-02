using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Enums;

namespace Pos.Web.Features.Orders.AddRefund
{
    public record AddRefundCommand(
        Guid OrderId, 
        decimal Amount, 
        DateTime PaymentDate, 
        PaymentMethod PaymentMethod, 
        string Reason, 
        string? TransactionId) : ICommand<Guid>;
}
