using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Orders.UpdateOrderDelivery
{
    public class UpdateOrderDeliveryValidator : AbstractValidator<UpdateOrderDeliveryCommand>
    {
        public UpdateOrderDeliveryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.CourierId)
                .NotEmpty();

            RuleFor(x => x.DeliveryAddress)
                .NotEmpty().WithMessage("Delivery Address is required.")
                .MaximumLength(200);

            RuleFor(x => x.DeliveryCity).MaximumLength(100);
            RuleFor(x => x.DeliveryRegion).MaximumLength(100);
            RuleFor(x => x.DeliveryCountry).MaximumLength(100);
            RuleFor(x => x.DeliveryPostalCode).MaximumLength(20);
            RuleFor(x => x.TrackingNumber).MaximumLength(100);
            RuleFor(x => x.Notes).MaximumLength(200);
        }
    }
}
