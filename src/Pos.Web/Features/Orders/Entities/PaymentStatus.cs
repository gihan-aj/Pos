namespace Pos.Web.Features.Orders.Entities
{
    public enum PaymentStatus
    {
        Unpaid = 1,
        Partial = 2,
        Paid = 3,
        Refunded = 4,
        Failed = 5,
    }
}
