using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.GetProductVariantList
{
    public static class GetProductVariantListEndpoint
    {
        public static void MapGetProductVariantList(this RouteGroupBuilder group)
        {
            group.MapGet("/variants", async ([AsParameters] GetProductVariantListQuery query, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.ToProblemDetails();
            })
            .WithName("GetProductVariantList")
            .WithSummary("Search and filter product variants")
            .Produces<PagedList<ProductVariantListItem>>(200);
        }
    }
}
