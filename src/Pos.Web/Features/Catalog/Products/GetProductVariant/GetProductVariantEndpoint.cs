using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.GetProductVariant
{
    public static class GetProductVariantEndpoint
    {
        public static void MapGetProductVariant(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}/variants/{variantId}", async (Guid id, Guid variantId, ISender sender) =>
            {
                var result = await sender.Send(new GetProductVariantQuery(id, variantId));
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .WithName("GetVariant")
            .WithSummary("Get product variant by id")
            .Produces<ProductVariantResponse>(200)
            .ProducesProblem(404);
        }
    }
}
