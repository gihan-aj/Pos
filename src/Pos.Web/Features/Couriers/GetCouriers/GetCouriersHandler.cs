using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Couriers.GetCouriers
{
    public class GetCouriersHandler : IQueryHandler<GetCouriersQuery, List<GetCouriersResponse>>
    {
        private readonly AppDbContext _dbContext;

        public GetCouriersHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<GetCouriersResponse>>> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Couriers
                .AsNoTracking();

            if(request.IsActive != null)
            {
                query = query.Where(x => x.IsActive);
            }

            var couriers = await query
                .OrderBy(c => c.Name)
                .Select(c => new GetCouriersResponse(c.Id,c.Name, c.PhoneNumber, c.IsActive))
                .ToListAsync(cancellationToken);

            return couriers;
        }
    }
}
