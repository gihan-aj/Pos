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
            RuleFor(x => x.Sku)
                .MinimumLength(3)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Sku));

            RuleForEach(x => x.Variants)
                .SetValidator(new CreateProductVariantDtoValidator());

            RuleForEach(x => x.Images)
                .SetValidator(new CreateProductImageDtoValidator());
        }
    }

    public class CreateProductVariantDtoValidator : AbstractValidator<CreateProductVariantDto>
    {
        public CreateProductVariantDtoValidator()
        {
            RuleFor(x => x.Size)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Color)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Sku)
                .MinimumLength(3)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Sku));

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Cost.HasValue);
        }
    }

    public class CreateProductImageDtoValidator : AbstractValidator<CreateProductImageDto>
    {
        public CreateProductImageDtoValidator()
        {
            RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(250);
        }
    }
}
