using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.UpdateCategory
{
    public static class UpdateCategoryEndpoint
    {
        public static void MapUpdateCategory(this RouteGroupBuilder group)
        {
            group.MapPut("/", async (UpdateCategoryCommand command, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
            .WithName("UodateCategory")
            .WithSummary("Update a category")
            .Produces(200)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
