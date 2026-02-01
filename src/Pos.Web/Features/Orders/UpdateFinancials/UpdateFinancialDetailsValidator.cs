using FluentValidation;

namespace Pos.Web.Features.Orders.UpdateFinancials
{
    public class UpdateFinancialDetailsValidator : AbstractValidator<UpdateFinancialDetailsCommand>
    {
        public UpdateFinancialDetailsValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();
            RuleFor(c => c.ShippingFee)
                .GreaterThanOrEqualTo(0).WithMessage("Shipping fee must be greater than or equal to zero.");
            RuleFor(c => c.TaxAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Tax amount must be greater than or equal to zero.");
            RuleFor(c => c.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount amount must be greater than or equal to zero.");
        }
    }
}
