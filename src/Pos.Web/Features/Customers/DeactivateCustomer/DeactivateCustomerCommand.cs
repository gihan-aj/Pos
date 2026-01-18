using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.DeactivateCustomer
{
    public record DeactivateCustomerCommand(Guid Id): ICommand;
}
