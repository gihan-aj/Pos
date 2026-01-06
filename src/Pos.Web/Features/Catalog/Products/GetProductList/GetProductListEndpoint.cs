using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Catalog.Products.GetProductList
{
    public static class GetProductListEndpoint
    {
        public static void MapGetProductList(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([AsParameters] GetProductsQuery query, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .WithName("GetProductList")
            .WithSummary("Search and filter products")
            .Produces<PagedList<ProductListItem>>(200);
        }
    }
}
