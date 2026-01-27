using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Couriers.CreateCourier
{
    public static class CreateCourierEndpoint
    {
        public static void MapCreateCourier(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateCourierCommand command, ISender mediator, CancellationToken cancellationToken = default) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.Ok(new { Id = result.Value })
                    : result.ToProblemDetails();
            })
            .WithName("CreateCourier")
            .WithSummary("Creates a new courier")
            .Produces(200)
            .ProducesProblem(409);
        }
    }
}
