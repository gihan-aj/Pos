using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public static class GetCategoryTreeEndpoint
    {
        [WolverineGet("/api/categories/{id}/tree")]
        [ProducesResponseType(typeof(CategoryTreeItem), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public static async Task<IResult> GetTree(
            Guid id, 
            bool? onlyActive, 
            IMessageBus bus, 
            CancellationToken cancellationToken = default)
        {
            var result = await bus.InvokeAsync<Result<CategoryTreeItem>>(new GetCategoryTreeQuery(id, onlyActive ?? false), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }
    }
}
