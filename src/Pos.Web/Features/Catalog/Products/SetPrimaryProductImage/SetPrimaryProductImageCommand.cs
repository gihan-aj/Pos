using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.SetPrimaryProductImage
{
    public record SetPrimaryProductImageCommand(Guid ProductId, Guid ImageId) : ICommand;
}
