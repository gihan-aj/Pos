using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.GetProduct
{
    public static class GetProductEndpoint
    {
        public static void MapGetProduct(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductQuery(id));
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .WithName("GetProduct")
            .WithSummary("Get product details by id with its variants and image urls")
            .Produces<ProductResponse>(200)
            .ProducesProblem(404);
        }
    }
}
