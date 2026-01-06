using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.Brand)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Brand));

            RuleFor(x => x.Material)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Material));

            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();

            // SKU validation: If provided manually, ensure it's not empty and follows some format if needed
            RuleFor(x => x.SkuOverride)
                .MinimumLength(3)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.SkuOverride));
        }
    }
}
