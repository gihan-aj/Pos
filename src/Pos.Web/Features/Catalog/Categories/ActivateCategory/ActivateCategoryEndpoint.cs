using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.ActivateCategory
{
    public static class ActivateCategoryEndpoint
    {
        public static void MapActivateCategory(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/activate", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new ActivateCategoryCommand(id), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
            .WithName("ActivateCategory")
            .WithSummary("Activated a deactivated category")
            .Produces(200)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
