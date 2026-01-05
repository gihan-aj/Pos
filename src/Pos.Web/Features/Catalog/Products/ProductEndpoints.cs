using Pos.Web.Features.Catalog.Products.CreateProduct;

namespace Pos.Web.Features.Catalog.Products
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products")
                .WithTags("Products");

            group.MapCreateProductEndpoint();
        }
    }
}
