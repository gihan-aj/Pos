namespace Pos.Web.Features.Orders.Entities
{
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        Completed = 4,
        Shipped = 5,
        Delivered = 6,
        Cancelled = 0,
    }
}
