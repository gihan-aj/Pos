using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public static class UpdateProductEndpoint
    {
        public static void MapUpdateProduct(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}", async (Guid id, UpdateProductCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.Problem(statusCode: 400, title: "Bad Request", detail: "ID mismatch");

                var result = await sender.Send(command);
                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            })
                .WithName("UpdateProduct")
                .WithSummary("Updates product main details")
                .Produces(200)
                .ProducesProblem(400)
                .ProducesProblem(404);
        }
    }
}
