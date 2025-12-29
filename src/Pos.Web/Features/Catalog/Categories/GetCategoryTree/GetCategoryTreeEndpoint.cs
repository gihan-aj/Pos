using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public static class GetCategoryTreeEndpoint
    {
        public static void MapGetCategoryTree(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}/tree", async (Guid id, bool? onlyActive, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCategoryTreeQuery(id, onlyActive ?? false), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetCategoryTree")
            .WithSummary("Get the category tree")
            .Produces(200, typeof(CategoryTreeItem))
            .ProducesProblem(404);
        }
    }
}
