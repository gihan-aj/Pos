using FluentValidation;

namespace Pos.Web.Features.Orders.ChangeOrderItemQuantity
{
    public class ChangeOrderItemQuantityValidator : AbstractValidator<ChangeOrderItemQuantityCommand>
    {
        public ChangeOrderItemQuantityValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID must not be empty.");
            RuleFor(x => x.OrderItemId)
                .NotEmpty().WithMessage("Order Item ID must not be empty.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
