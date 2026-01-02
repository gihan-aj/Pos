namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public record CategoryTreeItem(
        Guid Id, 
        string Name,
        bool IsActive,
        int DisplayOrder,
        List<CategoryTreeItem> Children);
}
