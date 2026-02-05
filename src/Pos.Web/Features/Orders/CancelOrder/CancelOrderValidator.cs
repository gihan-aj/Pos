using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.CancelOrder
{
    public class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID must be provided.");
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Cancellation reason must be provided.")
                .MaximumLength(200).WithMessage("Cancellation reason cannot exceed 200 characters.");
        }
    }
}
