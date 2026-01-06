namespace Pos.Web.Features.Catalog.Products.GetProduct
{
    public record ProductImageDto(
        Guid Id,
        string ImageUrl,
        bool IsPrimary,
        int DisplayOrder
    );
}
