using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Features.Couriers.CreateCourier
{
    public class CreateCourierValidator : AbstractValidator<CreateCourierCommand>
    {
        public CreateCourierValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(c => c.TrackingUrlTemplate)
                .MaximumLength(500);
            RuleFor(c => c.WebsiteUrl)
                .MaximumLength(250)
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Website URL must be a valid absolute URL.");
            RuleFor(c => c.PhoneNumber)
                .MaximumLength(50);
        }
    }
}
