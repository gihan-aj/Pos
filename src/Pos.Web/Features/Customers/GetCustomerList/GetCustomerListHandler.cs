using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.GetCustomerList
{
    public class GetCustomerListHandler : IQueryHandler<GetCustomerListQuery, PagedList<CustomerListItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetCustomerListHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedList<CustomerListItem>>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Customers.AsQueryable();


            // Is active
            if (request.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == request.IsActive.Value);
            }

            // Search 
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();
                if (string.Equals(request.SearchIn, "name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Name.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "phoneNumber", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.PhoneNumber.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "email", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Email != null && c.Email.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "address", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Address != null && c.Address.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "city", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.City != null && c.City.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "postalCode", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.PostalCode != null && c.PostalCode.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "region", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Region != null && c.Region.Contains(term));
                }
                else
                {
                    query = query.Where(c =>
                        c.Name.Contains(term)
                        || c.PhoneNumber.Contains(term)
                        || c.Email != null && c.Email.Contains(term));
                }                
                
            }

            // --- SORTING ---
            bool isAsc = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);
            query = request.SortBy?.ToLower() switch
            {
                "name" => isAsc ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                "phoneNumber" => isAsc ? query.OrderBy(c => c.PhoneNumber) : query.OrderByDescending(c => c.PhoneNumber),
                "email" => isAsc ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                "city" => isAsc ? query.OrderBy(c => c.City) : query.OrderByDescending(c => c.City),
                "created" => isAsc ? query.OrderBy(p => p.CreatedOnUtc) : query.OrderByDescending(p => p.CreatedOnUtc),
                _ => query.OrderByDescending(p => p.CreatedOnUtc) // Default
            };

            var projectedQuery = query.Select(c => new CustomerListItem(
                    c.Id,
                    c.Name,
                    c.PhoneNumber,
                    c.Email,
                    c.City,
                    c.IsActive
                ));

            var pagedList = await PagedList<CustomerListItem>.CreateAsync(projectedQuery, request.Page, request.PageSize, cancellationToken);

            return pagedList;
        }
    }
}
