using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.DeactivateCategory
{
    public record DeactivateCategoryCommand(Guid Id) : ICommand;
}
