using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.Brand)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Brand));

            RuleFor(x => x.Material)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Material));

            RuleFor(x => x.BasePrice)
                .GreaterThanOrEqualTo(0);

            RuleForEach(x => x.Variants)
                .SetValidator(new ProductVariantDtoValidator());

            RuleForEach(x => x.Images)
                .SetValidator(new ProductImageDtoValidator());
        }
    }

    public class ProductVariantDtoValidator : AbstractValidator<ProductVariantDto>
    {
        public ProductVariantDtoValidator()
        {
            RuleFor(x => x.Id).NotNull();

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

    public class ProductImageDtoValidator : AbstractValidator<ProductImageDto>
    {
        public ProductImageDtoValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(250);
        }
    }
}
