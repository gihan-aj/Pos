using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.RemoveProductVariant
{
    public static class RemoveProductVariantEndpoint 
    {
        public static void MapRemoveProductVariant(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id}/variants/{variantId}", async (Guid id, Guid variantId, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new RemoveProductVariantCommand(id, variantId));
                return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
            })
                .WithName("RemoveVariant")
                .WithSummary("Removes a variant from the product")
                .Produces(204)
                .ProducesProblem(404);
        }
    }
}
