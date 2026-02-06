using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Infrastructure.Services;

public class AppSequenceService : IAppSequenceService
{
    private readonly AppDbContext _context;

    public AppSequenceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GetNextNumberAsync(string sequenceId, CancellationToken cancellationToken = default)
    {
        // Use raw SQL with UPDLOCK to prevent concurrency issues
        var sequence = await _context.AppSequences
            .FromSqlRaw("SELECT * FROM AppSequences WITH (UPDLOCK) WHERE Id = {0}", sequenceId)
            .SingleOrDefaultAsync(cancellationToken);

        if (sequence == null)
        {
            throw new InvalidOperationException($"Sequence '{sequenceId}' not found.");
        }

        sequence.CurrentValue += sequence.Increment;

        // Note: We remain attached to the context, so modifications are tracked.
        // We do NOT call SaveChangesAsync here, allowing the caller to commit the transaction.

        return $"{sequence.Prefix}{sequence.CurrentValue}";
    }
}
