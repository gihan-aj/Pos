using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.ActivateProductVariant
{
    public static class ActivateProductVariantEndpoint
    {
        public static void MapActivateProductVariant(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/variants/{variantId}/activate", async (
                Guid id, 
                Guid variantId, 
                ISender mediator, 
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new ActivateProductVariantCommand(id, variantId), cancellationToken);

                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            })
                .WithName("ActivateVariant")
                .WithSummary("Activate a variant of a product")
                .Produces(200)
                .ProducesProblem(404);
        }
    }
}
