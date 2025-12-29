using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.DeleteCategory
{
    public static class DeleteCategoryEndpoint
    {
        public static void MapDeleteCategory(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

                return result.IsSuccess
                    ? Results.NoContent()
                    : result.ToProblemDetails();
            })
            .WithName("DeleteCategory")
            .WithSummary("Delete a orphan category")
            .Produces(200)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
