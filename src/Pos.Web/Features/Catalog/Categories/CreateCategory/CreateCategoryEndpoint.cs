using Microsoft.AspNetCore.Mvc;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public static class CreateCategoryEndpoint
    {
        [WolverinePost("/api/categories")]
        [ProducesResponseType(typeof(CreateCategoryResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 409)]
        public static async Task<IResult> Post(
            CreateCategoryCommand command,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            var result = await bus.InvokeAsync<Result<Category>>(command, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new CreateCategoryResponse(result.Value.Id, result.Value.Path))
                : result.ToProblemDetails();
        }
    }
}
