namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public record CategoryTreeItem(
        Guid Id, 
        string Name,
        bool IsActive,
        List<CategoryTreeItem> Children);
}
