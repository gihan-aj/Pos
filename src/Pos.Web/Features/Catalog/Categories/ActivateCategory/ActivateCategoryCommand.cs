using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.ActivateCategory
{
    public record ActivateCategoryCommand(Guid Id) : ICommand;
}
