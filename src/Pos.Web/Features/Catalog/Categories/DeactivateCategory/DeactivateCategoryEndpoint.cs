using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.DeactivateCategory
{
    public static class DeactivateCategoryEndpoint
    {
        public static void MapDeactivateCategory(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/deactivate", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeactivateCategoryCommand(id), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
            .WithName("DeactivateCategory")
            .WithSummary("Deactivated a category and its sub categories")
            .Produces(200)
            .ProducesProblem(404);
        }
    }
}
