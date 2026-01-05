using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.AddProductImage
{
    public static class AddProductImageEndpoint
    {
        public static void MapAddProductImage(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/images", async (Guid id, AddProductImageCommand command, ISender mediator, CancellationToken ct) =>
            {
                if (id != command.ProductId)
                {
                    return Results.Problem(statusCode: 400, title: "Bad Request", detail: "Route ID mismatch");
                }

                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
                .WithName("AddProductImage")
                .WithSummary("Adds an image to a product")
                .Produces(200)
                .ProducesProblem(400)
                .ProducesProblem(404);
        }
    }
}
