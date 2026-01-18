using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Orders.CreateOrder
{
    public static class CreateOrderEndpoint
    {
        public static void MapCreateOrder(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateOrderCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("CreateOrder")
            .WithSummary("Creates a new order")
            .Produces<Guid>(200)
            .ProducesProblem(404)
            .ProducesProblem(409);
        }
    }
}
