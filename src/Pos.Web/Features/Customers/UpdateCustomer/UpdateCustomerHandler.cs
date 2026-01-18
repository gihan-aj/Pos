using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Customers.UpdateCustomer
{
    public class UpdateCustomerHandler : ICommandHandler<UpdateCustomerCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .FindAsync(command.Id, cancellationToken);
            if(customer is null)
                return Result.Failure(new Error("Customer.NotFound", "Customer not found.", ErrorType.NotFound));

            if(customer.PhoneNumber != command.PhoneNumber)
            {
                var phoneNumberExists = await _dbContext.Customers
                    .AnyAsync(c => c.PhoneNumber == command.PhoneNumber && c.Id != command.Id, cancellationToken);
                if(phoneNumberExists)
                    return Result.Failure(new Error("Customer.PhoneNumberInUse", "Phone number already in use.", ErrorType.Conflict));
            }

            var updateResult = customer.UpdateDetails(
                command.Name,
                command.PhoneNumber,
                command.Email,
                command.Address,
                command.City,
                command.Country,
                command.PostalCode,
                command.Region,
                command.Notes);

            return updateResult;
        }
    }
}
