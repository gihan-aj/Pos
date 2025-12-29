using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Categories.GetCategory
{
    public record GetCategoryQuery(Guid Id, string[]? Includes) : IQuery<CategoryResponse>;
}
