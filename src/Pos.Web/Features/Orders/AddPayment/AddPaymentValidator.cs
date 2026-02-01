using FluentValidation;

namespace Pos.Web.Features.Orders.AddPayment
{
    public class AddPaymentValidator : AbstractValidator<AddPaymentCommand>
    {
        public AddPaymentValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Payment amount must be positive.");

            RuleFor(x => x.PaymentMethod).IsInEnum();

            RuleFor(x => x.Notes).MaximumLength(200);
            RuleFor(x => x.TransactionId).MaximumLength(100);
        }
    }
}
