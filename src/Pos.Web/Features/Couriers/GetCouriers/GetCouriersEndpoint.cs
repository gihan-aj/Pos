using MediatR;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Couriers.GetCouriers
{
    public static class GetCouriersEndpoint
    {
        public static void MapGetCouriers(this RouteGroupBuilder group)
        {
            group.MapGet("/all", async ([AsParameters] GetCouriersQuery query, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("GetAllCouriers")
            .WithSummary("Get summary details of all couriers")
            .Produces(200, typeof(List<GetCouriersResponse>));
        }
    }
}
