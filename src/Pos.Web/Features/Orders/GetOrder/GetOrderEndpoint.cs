using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.GetOrder
{
    public static class GetOrderEndpoint
    {
        public static void MapGetOrder(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (Guid id, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(new GetOrderQuery(id), cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetOrder")
            .WithSummary("Get a existing order by id")
            .Produces<GetOrderResponse>(200)
            .ProducesProblem(404);
        }
    }
}
