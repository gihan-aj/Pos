using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.SetPrimaryProductImage
{
    public static class SetPrimaryProductImageEndpoint
    {
        public static void MapSetPrimaryProductImage(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/image/{imageId}/primary", async (Guid id, Guid imageId, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new SetPrimaryProductImageCommand(id, imageId), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
                .WithName("SetPrimaryImage")
                .WithSummary("Sets a specific image as primary")
                .Produces(200)
                .ProducesProblem(404);
        }
    }
}
