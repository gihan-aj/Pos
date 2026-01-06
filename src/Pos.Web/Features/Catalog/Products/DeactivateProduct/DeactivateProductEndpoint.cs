using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.DeactivateProduct
{
    public static class DeactivateProductEndpoint
    {
        public static void MapDeactivateProduct(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/deactivate", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeactivateProductCommand(id));
                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            })
                .WithName("DeactivateProduct")
                .WithSummary("Deactivate a product")
                .Produces(200)
                .ProducesProblem(404);

        }
    }
}
