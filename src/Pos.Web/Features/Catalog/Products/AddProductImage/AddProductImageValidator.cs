using FluentValidation;

namespace Pos.Web.Features.Catalog.Products.AddProductImage
{
    public class AddProductImageValidator : AbstractValidator<AddProductImageCommand>
    {
        public AddProductImageValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(250);
        }
    }
}
