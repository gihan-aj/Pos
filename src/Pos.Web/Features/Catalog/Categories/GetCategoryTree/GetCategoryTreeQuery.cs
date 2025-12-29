using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public record GetCategoryTreeQuery(Guid RootId, bool OnlyActive) : IQuery<CategoryTreeItem>;
}
