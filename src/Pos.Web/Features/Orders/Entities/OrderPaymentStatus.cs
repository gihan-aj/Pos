namespace Pos.Web.Features.Orders.Entities
{
    public enum OrderPaymentStatus
    {
        Unpaid = 1,
        Partial = 2,
        Paid = 3,
        Refunded = 4,
        Overpaid = 5,
    }
}
