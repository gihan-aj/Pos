using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer Id is required.");
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one order item is required.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductVariantId)
                    .NotEmpty().WithMessage("Product Variant Id is required.");
                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            });

            RuleForEach(x => x.OrderPayments).ChildRules(payments =>
            {
                payments.RuleFor(p => p.PaymentMethod)
                    .NotEmpty().WithMessage("Payment Method is required.");
                payments.RuleFor(p => p.Amount)
                    .GreaterThan(0).WithMessage("Payment Amount must be greater than zero.");
            });
        }
    }
}
