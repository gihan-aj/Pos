using FluentValidation;

namespace Pos.Web.Features.Customers.CreateCustomer
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.Address)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.Address));
            RuleFor(x => x.City)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.City));
            RuleFor(x => x.Country)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Country));
            RuleFor(x => x.PostalCode)
                .MaximumLength(20)
                .When(x => !string.IsNullOrEmpty(x.PostalCode));
            RuleFor(x => x.Region)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Region));
            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }
}
