using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(250).When(x => !string.IsNullOrWhiteSpace(x.Description));
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Brand).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Brand));
            RuleFor(x => x.Material).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Material));
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
        }
    }
}
