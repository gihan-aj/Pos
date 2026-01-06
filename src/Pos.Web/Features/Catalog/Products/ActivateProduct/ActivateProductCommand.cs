using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.ActivateProduct
{
    public record ActivateProductCommand(Guid Id) : ICommand;
}
