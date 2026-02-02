using Pos.Web.Features.Orders.Entities;
using Pos.Web.Shared.Enums;

namespace Pos.Web.Features.Orders.GetOrder
{
    public sealed record OrderPaymentDto(
        Guid Id,
        DateTime PaymentDate,
        PaymentMethod PaymentMethod,
        decimal Amount,
        string? TransactionId,
        PaymentStatus Status
    );
}
