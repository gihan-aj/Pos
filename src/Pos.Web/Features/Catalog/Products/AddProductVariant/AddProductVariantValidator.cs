using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.AddProductVariant
{
    public class AddProductVariantValidator : AbstractValidator<AddProductVariantCommand>
    {
        public AddProductVariantValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Size).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Color).NotEmpty().MaximumLength(30);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).When(x => x.Cost.HasValue);

            RuleFor(x => x.SkuOverride)
                .MinimumLength(3)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.SkuOverride));
        }
    }
}
