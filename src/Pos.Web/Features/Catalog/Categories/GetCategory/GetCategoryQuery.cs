using Microsoft.AspNetCore.Mvc;

namespace Pos.Web.Features.Catalog.Categories.GetCategory
{
    public record GetCategoryQuery(Guid Id, string[]? Includes);
}
