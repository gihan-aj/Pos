namespace Pos.Web.Shared.Abstractions;

public interface IAppSequenceService
{
    Task<string> GetNextNumberAsync(string sequenceId, CancellationToken cancellationToken = default);
}
