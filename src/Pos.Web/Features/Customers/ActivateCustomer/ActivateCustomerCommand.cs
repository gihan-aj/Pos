using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.ActivateCustomer
{
    public record ActivateCustomerCommand(Guid Id) : ICommand;
}
