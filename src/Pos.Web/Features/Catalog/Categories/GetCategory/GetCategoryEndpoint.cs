using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.GetCategory
{
    public static class GetCategoryEndpoint
    {
        public static void MapGetCategory(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (Guid id, string[]? includes, ISender mediator, CancellationToken cancellationToken) =>
            {
                var query = new GetCategoryQuery(id, includes);
                var result = await mediator.Send(query, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetCategory")
            .WithSummary("Get a category by id")
            .Produces(200)
            .ProducesProblem(404);
        }
    }
}
