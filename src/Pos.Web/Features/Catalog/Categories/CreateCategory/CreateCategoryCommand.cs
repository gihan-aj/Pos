using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public record CreateCategoryCommand(
        string Name,
        string? Description,
        Guid? ParentCategoryId,
        int DisplayOrder,
        string? IconUrl,
        string? Color) :ICommand<Category>;
}
