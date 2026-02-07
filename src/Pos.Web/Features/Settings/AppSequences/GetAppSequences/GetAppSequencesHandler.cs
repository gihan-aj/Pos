using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Settings.AppSequences.GetAppSequences;

public class GetAppSequencesHandler : IQueryHandler<GetAppSequencesQuery, List<GetAppSequencesResponse>>
{
    private readonly AppDbContext _context;

    public GetAppSequencesHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<GetAppSequencesResponse>>> Handle(GetAppSequencesQuery request, CancellationToken cancellationToken)
    {
        var sequences = await _context.AppSequences
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = sequences.Select(s => new GetAppSequencesResponse(
            s.Id,
            s.Prefix,
            s.CurrentValue,
            s.Increment,
            $"{s.Prefix}{s.CurrentValue + s.Increment}"
        )).ToList();

        return Result.Success(response);
    }
}
