using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.ActivateCategory
{
    public static class ActivateCategoryEndpoint
    {
        [WolverinePost("/api/categories/{id}/activate")]
        public static async Task<IResult> Post(Guid id, IMessageBus bus, CancellationToken cancellationToken = default)
        {
            var result = await bus.InvokeAsync<Result>(new ActivateCategoryCommand(id), cancellationToken);
            return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
        }
    }
}
