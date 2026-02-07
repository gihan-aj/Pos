using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Shared.Extensions;

namespace Pos.Web.Features.Settings.AppSequences.UpdateAppSequence;

public static class UpdateAppSequenceEndpoint
{
    public static void MapUpdateAppSequence(this RouteGroupBuilder group)
    {
        group.MapPut("/sequences/{id}", async (string id, [FromBody] UpdateAppSequenceRequest request, ISender mediator, CancellationToken cancellationToken) =>
        {
            var command = new UpdateAppSequenceCommand(id, request.Prefix, request.CurrentValue, request.Increment);
            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Results.NoContent()
                : result.ToProblemDetails();
        })
        .WithName("UpdateAppSequence")
        .WithSummary("Update an application sequence")
        .Produces(204)
        .ProducesProblem(400)
        .ProducesProblem(404)
        .ProducesProblem(409);
    }
}

public class UpdateAppSequenceRequest
{
    public string Prefix { get; set; } = null!;
    public int CurrentValue { get; set; }
    public int Increment { get; set; }
}
