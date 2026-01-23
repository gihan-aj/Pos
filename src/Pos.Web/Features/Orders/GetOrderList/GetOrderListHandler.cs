using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.GetOrderList
{
    public class GetOrderListHandler : IQueryHandler<GetOrderListQuery, PagedList<OrderListItem>>
    {
        private readonly AppDbContext _dbContext;

        public GetOrderListHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedList<OrderListItem>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .AsQueryable();

            // --- FILTERING ---
            if (request.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == request.CustomerId.Value);

            if (request.Status.HasValue)
                query = query.Where(o => o.Status == request.Status.Value);

            if (request.PaymentStatus.HasValue)
                query = query.Where(o => o.PaymentStatus == request.PaymentStatus.Value);

            if (request.StartDate.HasValue)
                query = query.Where(o => o.OrderDate >= request.StartDate.Value);

            if(request.EndDate.HasValue)
            {
                // Ensuring to cover entire day (up to 23:59:59)
                var endOfDay = request.EndDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.OrderDate <= endOfDay);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();
                if (string.Equals(request.SearchIn, "orderNumber", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(o => o.OrderNumber.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "customerName", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(o => o.Customer!.Name != null && o.Customer.Name.Contains(term));
                }
                else if (string.Equals(request.SearchIn, "deliveryCity", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(o => o.DeliveryCity != null && o.DeliveryCity.Contains(term));
                }
                else
                {
                    query = query.Where(o =>
                        o.OrderNumber.Contains(term) ||
                        (o.Customer != null && o.Customer.Name.Contains(term)) ||
                        (o.DeliveryCity != null && o.DeliveryCity.Contains(term)));
                }
            }

            // --- SORTING ---
            bool isAsc = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            query = request.SortBy?.ToLower() switch
            {
                "orderNumber" => isAsc ? query.OrderBy(o => o.OrderNumber) : query.OrderByDescending(o => o.OrderNumber),
                "customerName" => isAsc ? query.OrderBy(o => o.Customer!.Name) : query.OrderByDescending(o => o.Customer!.Name),
                "totalAmount" => isAsc ? query.OrderBy(o => o.TotalAmount) : query.OrderByDescending(o => o.TotalAmount),
                "status" => isAsc ? query.OrderBy(o => o.Status) : query.OrderByDescending(o => o.Status),
                "orderDate" => isAsc ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate),
                _ => query.OrderByDescending(o => o.OrderDate) // Default: Newest first
            };

            // --- PROJECTION ---
            var projectedQuery = query.Select(o => new OrderListItem(
                o.Id,
                o.OrderNumber,
                o.Customer!.Name,
                o.OrderDate,
                o.Status,
                o.PaymentStatus,
                o.TotalAmount,
                o.OrderItems.Sum(oi => oi.Quantity), // EF Core translates this to a subquery count
                o.PaymentMethod,
                o.DeliveryCity
            ));

            // --- EXECUTION ---
            var pagedList = await PagedList<OrderListItem>.CreateAsync(
                projectedQuery,
                request.Page,
                request.PageSize,
                cancellationToken);

            return Result<PagedList<OrderListItem>>.Success(pagedList);
        }
    }
}
