namespace Pos.Web.Features.Orders.Entities
{
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 0,
    }
}
