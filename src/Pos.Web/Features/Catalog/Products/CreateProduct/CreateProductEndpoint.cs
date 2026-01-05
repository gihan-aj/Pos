using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.CreateProduct
{
    public static class CreateProductEndpoint
    {
        public static void MapCreateProduct(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateProductCommand command, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(new { Id = result.Value })
                    : result.ToProblemDetails();
            })
                .WithName("CreateProduct")
                .WithSummary("Creates a new base product")
                .Produces(200)
                .ProducesProblem(400)
                .ProducesProblem(404) // Category not found
                .ProducesProblem(409); // SKU Conflict;
        }
    }
}
