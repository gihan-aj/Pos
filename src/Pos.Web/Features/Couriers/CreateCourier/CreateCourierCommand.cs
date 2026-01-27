using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Couriers.CreateCourier
{
    public record CreateCourierCommand(
        string Name,
        string? TrackingUrlTemplate,
        string? WebsiteUrl,
        string? PhoneNumber) : ICommand<Guid>;
}
