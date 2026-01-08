namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public record CategoryTreeItem(
        Guid Id, 
        string Name,
        string NamePath,
        bool IsActive,
        int DisplayOrder,
        List<CategoryTreeItem> Children);
}
