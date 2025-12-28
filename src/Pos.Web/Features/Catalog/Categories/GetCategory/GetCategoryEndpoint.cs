using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.GetCategory
{
    public static class GetCategoryEndpoint
    {
        [WolverineGet("/api/categories/{id}")]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public static async Task<IResult> Get(
            Guid id,
            string[]? includes,
            IMessageBus bus,
            CancellationToken cancellationToken = default)
        {
            var query = new GetCategoryQuery(id, includes);
            var result = await bus.InvokeAsync<Result<CategoryResponse>>(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }
    }
}
