using MediatR;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Settings.AppSequences.UpdateAppSequence;

public record UpdateAppSequenceCommand(
    string Id,
    string Prefix,
    int CurrentValue,
    int Increment) : ICommand;
