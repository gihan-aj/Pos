using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();

            // SKU validation: If provided manually, ensure it's not empty and follows some format if needed
            RuleFor(x => x.SkuOverride)
                .MinimumLength(3)
                .When(x => !string.IsNullOrEmpty(x.SkuOverride));
        }
    }
}
