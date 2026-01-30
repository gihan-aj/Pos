using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Features.Couriers.Entities;
using Pos.Web.Features.Customers;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Enums;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.Entities
{
    public sealed class Order : AuditableEntity
    {
        private Order() { }

        private Order(
            string orderNumber,
            Guid customerId,
            string deliveryAddress,
            string? deliveryCity,
            string? deliveryRegion,
            string? deliveryCountry,
            string? deliveryPostalCode,
            PaymentStatus paymentStatus,
            string? notes,
            Guid? courierId,
            decimal shippingFee = 0,
            decimal taxAmount = 0,
            decimal discountAmount = 0)
        {
            Id = Guid.NewGuid();
            OrderNumber = orderNumber;
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            PaymentStatus = paymentStatus;
            DeliveryAddress = deliveryAddress;
            DeliveryCity = deliveryCity;
            DeliveryPostalCode = deliveryPostalCode;
            DeliveryRegion = deliveryRegion;
            DeliveryCountry = deliveryCountry;
            Notes = notes;
            CourierId = courierId;
            ShippingFee = shippingFee;
            TaxAmount = taxAmount;
            DiscountAmount = discountAmount;
            SubTotal = 0;
            TotalAmount = shippingFee;
            AmountDue = shippingFee;
        }

        public string OrderNumber { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public Customer? Customer { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }

        // -- Finacials --
        public decimal SubTotal { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal ShippingFee { get; private set; }
        public decimal TotalAmount { get; private set; }

        public decimal AmountPaid { get; private set; }
        public decimal AmountDue { get; private set; }

        // -- Delivery & Info --
        public string? PaymentMethod { get; private set; }
        public string DeliveryAddress { get; private set; } = string.Empty;
        public string? DeliveryCity { get; private set; }
        public string? DeliveryPostalCode { get; private set; }
        public string? DeliveryRegion { get; private set; }
        public string? DeliveryCountry { get; private set; }

        public Guid? CourierId { get; private set; }
        public Courier? Courier { get; private set; }
        public string? TrackingNumber { get; private set; }
        public string? Notes { get; private set; }

        // -- Items --
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // -- Payments --
        private readonly List<OrderPayment> _payments = new();
        public IReadOnlyCollection<OrderPayment> Payments => _payments.AsReadOnly();

        // --- FACTORY METHOD ---
        public static Result<Order> Create(
            string orderNumber,
            Guid customerId,
            string deliveryAddress,
            string? deliveryCity,
            string? deliveryRegion,
            string? deliveryCountry,
            string? deliveryPostalCode,
            PaymentStatus paymentStatus,
            string? notes,
            Guid? courierId,
            decimal shippingFee = 0,
            decimal taxAmount = 0,
            decimal discountAmount = 0)
        {
            if(customerId == Guid.Empty)
                return Result.Failure<Order>(Error.Validation("Order.InvalidCustomerId", "Customer ID cannot be empty."));

            if(string.IsNullOrEmpty(orderNumber))
                return Result.Failure<Order>(Error.Validation("Order.InvalidOrderNumber", "Order number cannot be empty."));

            if(string.IsNullOrEmpty(deliveryAddress))
                return Result.Failure<Order>(Error.Validation("Order.InvalidDeliveryAddress", "Delivery address cannot be empty."));

            return new Order(
                orderNumber,
                customerId,
                deliveryAddress,
                deliveryCity,
                deliveryRegion,
                deliveryCountry,
                deliveryPostalCode,
                paymentStatus,
                notes,
                courierId,
                shippingFee,
                taxAmount,
                discountAmount);
        }

        // --- DOMAIN BEHAVIOR ---
        public Result AddItem(Product product, ProductVariant variant, int quantity, decimal discountPerItem = 0)
        {
            if(quantity <= 0)
                return Result.Failure(Error.Validation("OrderItem.InvalidQuantity", "Quantity must be greater than zero."));

            if(product.Id != variant.ProductId)
                return Result.Failure(Error.Validation("OrderItem.VariantMismatch", "The product variant does not belong to the specified product."));

            var orderItem = new OrderItem(Id, product, variant, quantity, discountPerItem);

            _orderItems.Add(orderItem);

            RecalculateTotals();

            return Result.Success();
        }

        public void RecalculateTotals()
        {
            SubTotal = _orderItems.Sum(oi => oi.SubTotal);
            TotalAmount = SubTotal - DiscountAmount + TaxAmount + ShippingFee;

            RecalculatePaymentStatus();
        }

        public Result AssignCourier(Courier courier, string? trackingNumber)
        {
            if (courier is null)
                return Result.Failure(Error.Validation("Order.InvalidCourier", "Courier cannot be null."));

            if (!courier.IsActive)
                return Result.Failure(Error.Validation("Order.InactiveCourier", "Cannot assign an inactive courier."));

            CourierId = courier.Id;
            TrackingNumber = trackingNumber;

            return Result.Success();
        }

        public Result AddPayment(decimal amount, DateTime payementDate, PaymentMethod paymentMethod, string? transactionId, string? notes = null)
        {
            if (amount <= 0)
                return Result.Failure(Error.Validation("Payment.InvalidAmount", "Payment amount must be greater than zero."));

            //if (amount > AmountDue)
            //    return Result.Failure(Error.Validation("Payment.Overpayment", "Payment amount exceeds the amount due."));

            var payment = new OrderPayment(Id, amount, payementDate, paymentMethod, transactionId, notes);
            _payments.Add(payment);

            RecalculatePaymentStatus();

            return Result.Success();
        }

        private void RecalculatePaymentStatus()
        {
            AmountPaid = _payments
                .Where(p => p.IsSuccessful)
                .Sum(p => p.Amount);

            AmountDue = TotalAmount - AmountPaid;

            if (AmountPaid == 0)
                PaymentStatus = PaymentStatus.Unpaid;

            else if(AmountPaid >= TotalAmount)
                PaymentStatus = PaymentStatus.Paid;

            else
                PaymentStatus = PaymentStatus.Partial;
        }

        public Result UpdateDeliveryInfo(
            Guid courierId,
            string deliveryAddress,
            string? deliveryCity,
            string? deliveryRegion,
            string? deliveryCountry,
            string? deliveryPostalCode,
            string? trackingNumber,
            string? notes)
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Completed ||
                Status == OrderStatus.Cancelled)
            {
                return Result.Failure(Error.Validation("Order.CannotUpdate", $"Cannot update delivery details when order status is '{Status}'."));
            }

            if(string.IsNullOrWhiteSpace(deliveryAddress))
            {
                return Result.Failure(Error.Validation("Order.InvalidAddress", "Delivery address cannot be empty."));
            }

            CourierId = courierId;
            DeliveryAddress = deliveryAddress;
            DeliveryCity = deliveryCity;
            DeliveryRegion = deliveryRegion;
            DeliveryCountry = deliveryCountry;
            DeliveryPostalCode = deliveryPostalCode;
            TrackingNumber = trackingNumber;
            Notes = notes;

            return Result.Success(); 
        }
    }
}
