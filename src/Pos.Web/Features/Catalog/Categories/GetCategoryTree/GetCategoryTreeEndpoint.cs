using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryTree
{
    public static class GetCategoryTreeEndpoint
    {
        public static void MapGetCategoryTree(this RouteGroupBuilder group)
        {
            group.MapGet("/tree", async (Guid? id, bool? isActive, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(new GetCategoryTreeQuery(id ?? null, isActive ?? false), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetCategoryTree")
            .WithSummary("Get full category tree or sub-tree")
            .Produces<List<CategoryTreeItem>>(200)
            .ProducesProblem(404);
        }
    }
}
