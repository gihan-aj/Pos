using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Customers.CreateCustomer
{
    public class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand, Guid>
    {
        private readonly AppDbContext _dbContext;

        public CreateCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            bool phoneNumberExists = await _dbContext.Customers
                .AnyAsync(c => c.PhoneNumber == command.PhoneNumber, cancellationToken);
            if (phoneNumberExists)
                return Result<Guid>.Failure(
                    new Error("Customer.PhoneNumberExists", "A customer with the same phone number already exists.", ErrorType.Conflict));

            var customerResult = Customer.Create(
                command.Name,
                command.PhoneNumber,
                command.Email,
                command.Address,
                command.City,
                command.Country,
                command.PostalCode,
                command.Region,
                command.Notes);
            if (customerResult.IsFailure)
            {
                return Result<Guid>.Failure(customerResult.Error);
            }

            var customer = customerResult.Value;
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
