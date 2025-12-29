using FluentValidation;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100);

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Color)
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .When(x => !string.IsNullOrEmpty(x.Color))
                .WithMessage("Color must be a valid Hex code (e.g., #FF0000)");
        }
    }
}
