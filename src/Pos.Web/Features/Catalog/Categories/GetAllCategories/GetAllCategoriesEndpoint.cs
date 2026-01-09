using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Categories.GetAllCategories
{
    public static class GetAllCategoriesEndpoint
    {
        public static void MapGetAllCategories(this RouteGroupBuilder group)
        {
            group.MapGet("/all", async ([AsParameters] GetAllCategoriesQuery query, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetAllCategories")
            .WithSummary("Get summery details of all active categories")
            .Produces(200, typeof(List<CategorySummaryItem>));
        }
    }
}
