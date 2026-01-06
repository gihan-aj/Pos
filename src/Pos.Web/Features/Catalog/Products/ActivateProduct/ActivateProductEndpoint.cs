using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.ActivateProduct
{
    public static class ActivateProductEndpoint
    {
        public static void MapActivateProduct(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/activate", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new ActivateProductCommand(id));
                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            })
                .WithName("ActivateProduct")
                .WithSummary("Activate a product")
                .Produces(200)
                .ProducesProblem(404);

        }
    }
}
