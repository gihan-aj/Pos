using Pos.Web.Features.Catalog.Categories.ActivateCategory;
using Pos.Web.Features.Catalog.Categories.CreateCategory;
using Pos.Web.Features.Catalog.Categories.DeactivateCategory;
using Pos.Web.Features.Catalog.Categories.DeleteCategory;
using Pos.Web.Features.Catalog.Categories.GetCategory;
using Pos.Web.Features.Catalog.Categories.GetCategoryList;
using Pos.Web.Features.Catalog.Categories.GetCategoryTree;
using Pos.Web.Features.Catalog.Categories.UpdateCategory;

namespace Pos.Web.Features.Catalog.Categories
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories")
                .WithTags("Categories");

            group.MapCreateCategory();
            group.MapActivateCategory();
            group.MapDeactivateCategory();
            group.MapDeleteCategory();
            group.MapGetCategory();
            group.MapGetCategoryList();
            group.MapGetCategoryTree();
            group.MapUpdateCategory();
        }
    }
}
