using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.GetOrder
{
    public record GetOrderQuery(Guid Id) : IQuery<GetOrderResponse>;
}
