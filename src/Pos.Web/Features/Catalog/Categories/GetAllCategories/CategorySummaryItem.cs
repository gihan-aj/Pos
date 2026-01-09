namespace Pos.Web.Features.Catalog.Categories.GetAllCategories
{
    public record CategorySummaryItem(Guid Id, string Name, string NamePath, int Level, int DisplayOrder);
}
