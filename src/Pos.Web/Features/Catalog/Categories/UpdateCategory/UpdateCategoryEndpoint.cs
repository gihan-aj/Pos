using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.UpdateCategory
{
    public static class UpdateCategoryEndpoint
    {
        [WolverinePut("/api/categories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 409)]
        public static async Task<IResult> Put(UpdateCategoryCommand command, IMessageBus bus, CancellationToken cancellationToken)
        {
            var result = await bus.InvokeAsync<Result>(command, cancellationToken);
            return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
        }
    }
}
