namespace Pos.Web.Features.Orders.Entities
{
    public enum OrderItemStatus
    {
        Pending = 1,           // Not yet processed
        Reserved = 2,          // Stock reserved from inventory (in stock items)
        InProduction = 3,      // Being made/manufactured (out of stock items)
        Ready = 4,             // Item ready to pack
        Packed = 5,            // Item packed
        Shipped = 6,           // Item shipped
        Delivered = 7,         // Item delivered
        Cancelled = 0
    }
}
