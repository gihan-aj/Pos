using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Customers.GetCustomer
{
    public class GetCustomerHandler : IQueryHandler<GetCustomerQuery, GetCustomerResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetCustomerResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .FindAsync(request.Id, cancellationToken);
            if (customer is null)
                return Result.Failure<GetCustomerResponse>(new Error("Customer.NotFound", "Customer not found.", ErrorType.NotFound));

            return new GetCustomerResponse(
                customer.Id,
                customer.Name,
                customer.PhoneNumber,
                customer.Email,
                customer.Address,
                customer.City,
                customer.Country,
                customer.PostalCode,
                customer.Region,
                customer.Notes,
                customer.IsActive);
        }
    }
}
