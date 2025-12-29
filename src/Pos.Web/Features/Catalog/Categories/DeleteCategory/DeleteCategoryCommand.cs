using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : ICommand;
}
