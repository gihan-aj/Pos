using MediatR;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.GetCategoryList
{
    public static class GetCategoryListEndpoint
    {
        public static void MapGetCategoryList(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([AsParameters] GetCategoriesQuery query, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetCategoryList")
            .WithSummary("Get paginated category list")
            .Produces(200, typeof(PagedList<CategoryListItem>));
        }
    }
}
