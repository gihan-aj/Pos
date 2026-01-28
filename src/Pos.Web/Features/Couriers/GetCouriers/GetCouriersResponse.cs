namespace Pos.Web.Features.Couriers.GetCouriers
{
    public record GetCouriersResponse(
        Guid Id,
        string Name,
        string? PhoneNumber,
        bool IsActive
    );
}
