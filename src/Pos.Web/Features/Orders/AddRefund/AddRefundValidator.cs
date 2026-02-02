using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.AddRefund
{
    public class AddRefundValidator : AbstractValidator<AddRefundCommand>
    {
        public AddRefundValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Refund amount must be greater than zero.");
            RuleFor(x => x.Reason).NotEmpty().WithMessage("Refund reason is required.");
        }
    }
}
