using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Customers.ActivateCustomer
{
    public class ActivateCustomerHandler : ICommandHandler<ActivateCustomerCommand>
    {
        private readonly AppDbContext _dbContext;

        public ActivateCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(ActivateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .FindAsync(command.Id, cancellationToken);
            if (customer is null)
                return Result.Failure(new Error("Customer.NotFound", "Customer not found.", ErrorType.NotFound));

            if(customer.IsActive)
                return Result.Success();

            // VAlidations ????
            customer.Activate();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        
    }
}
