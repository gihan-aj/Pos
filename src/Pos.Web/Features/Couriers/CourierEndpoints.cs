using Pos.Web.Features.Couriers.CreateCourier;
using Pos.Web.Features.Couriers.GetCouriers;

namespace Pos.Web.Features.Couriers
{
    public static class CourierEndpoints
    {
        public static void MapCourierEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/couriers")
                .WithTags("Couriers");

            group.MapCreateCourier();
            group.MapGetCouriers();
        }
    }
}
