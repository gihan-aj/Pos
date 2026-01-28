namespace Pos.Web.Features.Orders.GetOrder
{
    public record CustomerDto(
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
