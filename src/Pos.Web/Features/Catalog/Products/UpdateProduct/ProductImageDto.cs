namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public record ProductImageDto(
        Guid Id,
        string ImageUrl,
        bool IsPrimary);
}
