using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Enums;

namespace Pos.Web.Features.Orders.Entities
{
    public sealed class OrderPayment : AuditableEntity
    {
        private OrderPayment() { }

        // Constructor for Internal/Cash payments (Instant Success)
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
            Method = paymentMethod;
            TransactionId = transactionId;
            Notes = notes;

            Provider = "System";
            Status = PaymentStatus.Completed;
        }

        // Constructor for Online/Future payments (Starts as Pending)
        internal OrderPayment(
            Guid orderId, 
            decimal amount, 
            string provider, 
            string? externalId)
        {
            Id = Guid.NewGuid(); 
            OrderId = orderId;
            Amount = amount;
            PaymentDate = DateTime.UtcNow;

            Method = PaymentMethod.Online;
            Provider = provider;
            TransactionId = externalId;

            Status = PaymentStatus.Pending;
        }

        public Guid OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }

        public PaymentMethod Method { get; private set; }
        public string Provider { get; private set; } = string.Empty; 

        public string? TransactionId { get; private set; } 
        public string? GatewayResponse { get; private set; } 
        public string? Notes { get; private set; }

        public PaymentStatus Status { get; private set; }

        // --- BEHAVIORS ---
        public void MarkAsCompleted(string? externalId, string? responseJson)
        {
            if (Status != PaymentStatus.Pending)
                return; // Idempotency check

            Status = PaymentStatus.Completed;
            if(externalId != null)
                TransactionId = externalId;

            GatewayResponse = responseJson;
        }

        public void MarkAsFailed(string? responseJson)
        {
            if (Status != PaymentStatus.Pending) 
                return;

            Status = PaymentStatus.Failed;
            GatewayResponse = responseJson;
        }

        public void Void()
        {
            // ONLY VOID A COMPLETED TRANSACTION IF IT HASN'T BEEN SETTLED.
            if(Status != PaymentStatus.Voided)
                Status = PaymentStatus.Voided;
        }
    }
}
