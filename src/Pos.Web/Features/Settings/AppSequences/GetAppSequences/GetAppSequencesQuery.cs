using MediatR;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Settings.AppSequences.GetAppSequences;

public record GetAppSequencesQuery : IQuery<List<GetAppSequencesResponse>>;

public record GetAppSequencesResponse(
    string Id,
    string Prefix,
    int CurrentValue,
    int Increment,
    string PreviewNext);
