using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;
using Wolverine;
using Wolverine.Http;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public static class CreateCategoryEndpoint
    {
        [WolverinePost("/api/categories")]
        public static async Task<IResult> Post(
            CreateCategoryCommand command,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            var result = await bus.InvokeAsync<Result<Guid>>(command, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new CreateCategoryResponse(result.Value))
                : result.ToProblemDetails();
        }
    }
}
