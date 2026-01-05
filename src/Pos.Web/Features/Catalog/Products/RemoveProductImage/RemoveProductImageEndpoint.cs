using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.RemoveProductImage
{
    public static class RemoveProductImageEndpoint
    {
        public static void MapRemoveProductImage(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id}/image/{imageId}", async (Guid id, Guid imageId, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new RemoveProductImageCommand(id, imageId), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
                .WithName("RemoveProductImage")
                .WithSummary("Removes an image from the product")
                .Produces(204)
                .ProducesProblem(404);
        }
    }
}
