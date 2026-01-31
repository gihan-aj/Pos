using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.AddOrderItem
{
    public class AddOrderItemValidator : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required.");
            RuleFor(x => x.ProductVariantId)
                .NotEmpty().WithMessage("Product Variant ID is required.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
