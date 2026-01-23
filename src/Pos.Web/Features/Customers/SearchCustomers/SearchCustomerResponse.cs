namespace Pos.Web.Features.Customers.SearchCustomers
{
    public record SearchCustomerResponse(
        Guid Id,
        string Name,
        string PhoneNumber,
        string? Email,
        string? Address,
        string? City,
        string? Country,
        string? PostalCode,
        string? Region,
        string? Notes);
}
