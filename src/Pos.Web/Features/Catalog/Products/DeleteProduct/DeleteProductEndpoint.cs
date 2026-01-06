using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.DeleteProduct
{
    public static class DeleteProductEndpoint
    {
        public static void MapDeleteProduct(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));
                return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
            })
                .WithName("DeleteProduct")
                .WithSummary("Delete a product")
                .Produces(204)
                .ProducesProblem(404)
                .ProducesProblem(409);

        }
    }
}
