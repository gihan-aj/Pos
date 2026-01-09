using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery(bool? OnlyLeafCategories) : IQuery<List<CategorySummaryItem>>;
}
