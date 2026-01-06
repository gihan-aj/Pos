using Pos.Web.Features.Catalog.Products.ActivateProduct;
using Pos.Web.Features.Catalog.Products.ActivateProductVariant;
using Pos.Web.Features.Catalog.Products.AddProductImage;
using Pos.Web.Features.Catalog.Products.AddProductVariant;
using Pos.Web.Features.Catalog.Products.CreateProduct;
using Pos.Web.Features.Catalog.Products.DeactivateProduct;
using Pos.Web.Features.Catalog.Products.DeactivateProductVariant;
using Pos.Web.Features.Catalog.Products.GetProduct;
using Pos.Web.Features.Catalog.Products.GetProductList;
using Pos.Web.Features.Catalog.Products.RemoveProductImage;
using Pos.Web.Features.Catalog.Products.RemoveProductVariant;
using Pos.Web.Features.Catalog.Products.SetPrimaryProductImage;
using Pos.Web.Features.Catalog.Products.UpdateProduct;
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
            group.MapUpdateProduct();
            group.MapActivateProduct();
            group.MapDeactivateProduct();
            group.MapGetProduct();
            group.MapGetProductList();

            group.MapAddProductVariant();
            group.MapUpdateProductVariant();
            group.MapRemoveProductVariant();
            group.MapActivateProductVariant();
            group.MapDeactivateProductVariant();

            group.MapAddProductImage();
            group.MapSetPrimaryProductImage();
            group.MapRemoveProductImage();
        }
    }
}
