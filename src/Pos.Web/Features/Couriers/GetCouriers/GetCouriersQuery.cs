using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Couriers.GetCouriers
{
    public record GetCouriersQuery(string? Search, bool? IsActive) : IQuery<List<GetCouriersResponse>>;
}
