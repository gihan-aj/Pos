namespace Pos.Web.Features.Customers.GetCustomer
{
    public record GetCustomerResponse(
        Guid Id,
        string Name,
        string PhoneNumber,
        string? Email,
        string? Address,
        string? City,
        string? Country,
        string? PostalCode,
        string? Region,
        string? Notes,
        bool IsActive);
}
