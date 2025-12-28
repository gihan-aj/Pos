namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public record CategoryTreeItem(Guid Id, string Name, List<CategoryTreeItem> Children);
}
