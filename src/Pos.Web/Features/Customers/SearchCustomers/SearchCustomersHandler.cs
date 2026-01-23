using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.SearchCustomers
{
    public class SearchCustomersHandler : IQueryHandler<SearchCustomersQuery, List<SearchCustomerResponse>>
    {
        private readonly AppDbContext _dbContext;

        public SearchCustomersHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<SearchCustomerResponse>>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Customers
                .AsNoTracking()
                .Where(c => c.IsActive)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLowerInvariant();

                query = query
                    .Where(c => 
                        c.Name.Contains(term) || 
                        c.PhoneNumber.Contains(term) || 
                        (!string.IsNullOrEmpty(c.Email) && c.Email.Contains(term)) ||
                        (!string.IsNullOrEmpty(c.City) && c.City.Contains(term))
                    );
            }

            var response = await query
                .Select(c => new SearchCustomerResponse(
                    c.Id,
                    c.Name,
                    c.PhoneNumber,
                    c.Email,
                    c.Address,
                    c.City,
                    c.Country,
                    c.PostalCode,
                    c.Region,
                    c.Notes))
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}
