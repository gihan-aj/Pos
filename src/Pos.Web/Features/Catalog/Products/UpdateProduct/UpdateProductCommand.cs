using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string? Description,
        Guid CategoryId,
        string? Brand,
        string? Material,
        Gender? Gender,
        decimal BasePrice,
        List<string> Tags,
        List<ProductVariantDto> Variants,
        List<ProductImageDto> Images) : ICommand;
}
