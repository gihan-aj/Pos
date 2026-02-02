using Pos.Web.Features.Orders.Entities;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.GetOrderList
{
    public record GetOrderListQuery: PagedRequest, IQuery<PagedList<OrderListItem>>
    {
        public Guid? CustomerId { get; init; }
        public OrderStatus? Status { get; init; }
        public OrderPaymentStatus? PaymentStatus { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string? SearchIn { get; init; } = null;
    }
}
