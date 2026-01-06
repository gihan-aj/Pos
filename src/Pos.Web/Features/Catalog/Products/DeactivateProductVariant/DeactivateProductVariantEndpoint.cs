using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.DeactivateProductVariant
{
    public static class DeactivateProductVariantEndpoint
    {
        public static void MapDeactivateProductVariant(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/variants/{variantId}/deactivate", async (
                Guid id,
                Guid variantId,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeactivateProductVariantCommand(id, variantId), cancellationToken);

                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            })
                .WithName("DeactivateVariant")
                .WithSummary("Deactivate a variant of a product")
                .Produces(200)
                .ProducesProblem(404);
        }
    }
}
