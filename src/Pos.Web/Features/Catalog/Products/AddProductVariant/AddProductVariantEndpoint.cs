using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.AddProductVariant
{
    public static class AddProductVariantEndpoint
    {
        public static void MapAddProductVariant(this RouteGroupBuilder group)
        {
            group.MapPost("/{id}/variants", async (Guid id, AddProductVariantCommand command, ISender mediator, CancellationToken ct) =>
            {
                if(id != command.ProductId)
                {
                    return Results.Problem(
                     statusCode: 400,
                     title: "Bad Request",
                     detail: "Route ID does not match Body ID");
                }

                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails();
            })
                .WithName("AddProductVariant")
                .WithSummary("Adds a new size/color variant to a product")
                .Produces(200)
                .ProducesProblem(400)
                .ProducesProblem(404)
                .ProducesProblem(409);
        } 
    }
}
