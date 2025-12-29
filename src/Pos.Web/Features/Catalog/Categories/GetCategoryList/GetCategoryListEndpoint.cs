using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryList
{
    public static class GetCategoryListEndpoint
    {
        [WolverineGet("/api/categories")]
        [ProducesResponseType(typeof(PagedList<CategoryListItem>), 200)]
        public static async Task<IResult> Get(
            [AsParameters] GetCategoriesQuery query,
            IMessageBus bus,
            CancellationToken cancellationToken = default)
        {
            var result = await bus.InvokeAsync<Result<PagedList<CategoryListItem>>>(query);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }
    }
}
