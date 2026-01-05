using Pos.Web.Features.Catalog.Products.AddProductImage;
using Pos.Web.Features.Catalog.Products.AddProductVariant;
using Pos.Web.Features.Catalog.Products.CreateProduct;
using Pos.Web.Features.Catalog.Products.RemoveProductImage;
using Pos.Web.Features.Catalog.Products.RemoveProductVariant;
using Pos.Web.Features.Catalog.Products.SetPrimaryProductImage;
using Pos.Web.Features.Catalog.Products.UpdateProductVariant;

namespace Pos.Web.Features.Catalog.Products
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products")
                .WithTags("Products");

            group.MapCreateProduct();

            group.MapAddProductVariant();
            group.MapUpdateProductVariant();
            group.MapRemoveProductVariant();

            group.MapAddProductImage();
            group.MapSetPrimaryProductImage();
            group.MapRemoveProductImage();
        }
    }
}
