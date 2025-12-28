using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.DeactivateCategory
{
    public static class DeactivateCategoryEndpoint
    {
        [WolverinePost("/api/categories/{id}/deactivate")]
        public static async Task<IResult> Post(Guid id, IMessageBus bus, CancellationToken cancellationToken = default)
        {
            var result = await bus.InvokeAsync<Result>(new DeactivateCategoryCommand(id));
            return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
        }
    }
}
