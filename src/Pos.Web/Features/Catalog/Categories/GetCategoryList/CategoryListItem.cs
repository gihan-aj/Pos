namespace Pos.Web.Features.Catalog.Categories.GetCategoryList
{
    public record CategoryListItem(
        Guid Id,
        string Name,
        string Breadcrumb,
        string? Description,
        bool IsActive,
        int Level,
        int DisplayOrder,
        Guid? ParentCategoryId,
        int? ProductCount // Useful for UI (disable delete button, etc.)
    );
}
