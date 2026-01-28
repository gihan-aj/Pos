using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Couriers.GetCouriers
{
    public record GetCouriersQuery(bool? IsActive) : IQuery<List<GetCouriersResponse>>;
}
