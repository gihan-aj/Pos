using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.UpdateProductVariant
{
    public static class UpdateProductVariantEndpoint
    {
        public static void MapUpdateProductVariant(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/variants/{variantId}", async (
                Guid id, 
                Guid variantId, 
                UpdateProductVariantCommand command, 
                ISender mediator, 
                CancellationToken cancellationToken) =>
            {
                if(id != command.ProductId || variantId != command.VariantId)
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "Route IDs mismatch");

                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            })
                .WithName("UpdateVariant")
                .WithSummary("Updates variant details (Price, Stock, SKU)")
                .Produces(200)
                .ProducesProblem(400)
                .ProducesProblem(404)
                .ProducesProblem(409);
        }
    }
}
