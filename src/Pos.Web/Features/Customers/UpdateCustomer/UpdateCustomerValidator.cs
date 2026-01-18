using FluentValidation;

namespace Pos.Web.Features.Customers.UpdateCustomer
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(x => x.Email)
                .MaximumLength(100)
                .EmailAddress()
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
