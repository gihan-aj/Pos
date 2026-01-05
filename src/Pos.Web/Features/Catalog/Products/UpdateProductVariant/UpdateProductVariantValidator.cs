using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.UpdateProductVariant
{
    public class UpdateProductVariantValidator : AbstractValidator<UpdateProductVariantCommand>
    {
        public UpdateProductVariantValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.VariantId).NotEmpty();
            RuleFor(x => x.Size).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Color).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Sku).NotEmpty().MinimumLength(3);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).When(x => x.Cost.HasValue);
        }
    }
}
