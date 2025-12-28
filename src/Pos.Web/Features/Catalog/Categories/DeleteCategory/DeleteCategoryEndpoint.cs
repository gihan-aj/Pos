using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.DeleteCategory
{
    public static class DeleteCategoryEndpoint
    {
        [WolverineDelete("/api/categories/{id}")]
        public static async Task<IResult> Delete(Guid id, IMessageBus bus)
        {
            var result = await bus.InvokeAsync<Result>(new DeleteCategoryCommand(id));
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        }
    }
}
