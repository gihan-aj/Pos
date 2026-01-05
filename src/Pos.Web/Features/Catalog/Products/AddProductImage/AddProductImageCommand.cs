using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.AddProductImage
{
    public record AddProductImageCommand(Guid ProductId, string ImageUrl, bool IsPrimary) : ICommand;
}
