using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Enums;

namespace Pos.Web.Features.Orders.Entities
{
    public sealed class OrderPayment : AuditableEntity
    {
        private OrderPayment() { }

        internal OrderPayment(
            Guid orderId,
            decimal amount,
            DateTime paymentDate,
            PaymentMethod paymentMethod,
            string? transactionId,
            string? notes)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            TransactionId = transactionId;
            Notes = notes;
            IsSuccessful = true;
        }

        public Guid OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public string? TransactionId { get; private set; } // Stripe/PayPal ID
        public string? Notes { get; private set; }
        public bool IsSuccessful { get; private set; }
    }
}
