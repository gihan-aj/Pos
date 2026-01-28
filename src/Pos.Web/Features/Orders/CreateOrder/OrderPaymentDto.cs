using Pos.Web.Shared.Enums;

namespace Pos.Web.Features.Orders.CreateOrder
{
    public record OrderPaymentDto(
        decimal Amount, 
        DateTime PaymentDate,
        PaymentMethod PaymentMethod,
        string? TransactionId,
        string Notes);
}
