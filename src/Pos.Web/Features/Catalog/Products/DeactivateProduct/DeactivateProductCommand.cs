using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.DeactivateProduct
{
    public record DeactivateProductCommand(Guid Id) : ICommand;
}
