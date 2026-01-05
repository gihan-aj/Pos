using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(
        Guid Id, 
        string Name, 
        string? Description, 
        Guid? ParentCategoryId,
        int DisplayOrder,
        string? IconUrl,
        string? Color) : ICommand;
}
