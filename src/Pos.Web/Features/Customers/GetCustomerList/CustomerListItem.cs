namespace Pos.Web.Features.Customers.GetCustomerList
{
    public record CustomerListItem(
        Guid Id,
        string Name,
        string PhoneNumber,
        string? Email,
        string? City,
        bool IsActive
    );
}
