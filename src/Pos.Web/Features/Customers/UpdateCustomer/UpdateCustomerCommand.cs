using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Customers.UpdateCustomer
{
    public record UpdateCustomerCommand(
        Guid Id,
        string Name,
        string PhoneNumber,
        string? Email,
        string? Address,
        string? City,
        string? Country,
        string? PostalCode,
        string? Region,
        string? Notes) : ICommand;
}
