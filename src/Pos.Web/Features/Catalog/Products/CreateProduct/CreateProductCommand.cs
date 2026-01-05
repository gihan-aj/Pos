using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string? Description,
        Guid CategoryId,
        string? Brand,
        string? Material,
        Gender? Gender,
        decimal BasePrice,
        List<string>? Tags,
        string? SkuOverride // If Null auto generate
    ) : ICommand<Guid>;
}
