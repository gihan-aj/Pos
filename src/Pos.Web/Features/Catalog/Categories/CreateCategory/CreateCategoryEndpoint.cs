using MediatR;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.CreateCategory
{
    public static class CreateCategoryEndpoint
    {
        public static void MapCreateCategory(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateCategoryCommand command, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(new CreateCategoryResponse(result.Value.Id, result.Value.Path))
                    : result.ToProblemDetails();
            })
            .WithName("CreateCategory")
            .WithSummary("Creates a new product category")
            .Produces(200)
            .ProducesProblem(400)
            .ProducesProblem(409);
        }
    }
}
