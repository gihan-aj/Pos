using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.UpdateFinancials
{
    public record UpdateFinancialDetailsCommand(
        Guid OrderId,
        decimal ShippingFee,
        decimal TaxAmount,
        decimal DiscountAmount) : ICommand;
}
