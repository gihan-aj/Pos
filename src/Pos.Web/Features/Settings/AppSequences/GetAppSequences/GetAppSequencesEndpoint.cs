using MediatR;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Settings.AppSequences.GetAppSequences;

public static class GetAppSequencesEndpoint
{
    public static void MapGetAppSequences(this RouteGroupBuilder group)
    {
        group.MapGet("/sequences", async (ISender mediator, CancellationToken cancellationToken) =>
        {
            var query = new GetAppSequencesQuery();
            var result = await mediator.Send(query, cancellationToken);
            
            return result.IsSuccess 
                ? Results.Ok(result.Value) 
                : result.ToProblemDetails();
        })
        .WithName("GetAppSequences")
        .WithSummary("Get all application sequences")
        .Produces<List<GetAppSequencesResponse>>();
    }
}
