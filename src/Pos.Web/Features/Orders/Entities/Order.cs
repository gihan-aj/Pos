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
            OrderPaymentStatus paymentStatus,
            string? notes,
            Guid? courierId,
            decimal shippingFee = 0,
            decimal taxAmount = 0,

            decimal discountAmount = 0,
            bool isCashOnDelivery = false)
        {
            Id = Guid.NewGuid();
            OrderNumber = orderNumber;
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            OrderPaymentStatus = paymentStatus;
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
            IsCashOnDelivery = isCashOnDelivery;
            SubTotal = 0;
            TotalAmount = shippingFee;
            AmountDue = shippingFee;
        }

        public string OrderNumber { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public Customer? Customer { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public OrderPaymentStatus OrderPaymentStatus { get; private set; }
        public bool IsCashOnDelivery { get; private set; }

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
            OrderPaymentStatus paymentStatus,
            string? notes,
            Guid? courierId,
            decimal shippingFee = 0,
            decimal taxAmount = 0,

            decimal discountAmount = 0,
            bool isCashOnDelivery = false)
        {
            if (customerId == Guid.Empty)
                return Result.Failure<Order>(Error.Validation("Order.InvalidCustomerId", "Customer ID cannot be empty."));

            if (string.IsNullOrEmpty(orderNumber))
                return Result.Failure<Order>(Error.Validation("Order.InvalidOrderNumber", "Order number cannot be empty."));

            if (string.IsNullOrEmpty(deliveryAddress))
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
                discountAmount,
                isCashOnDelivery);
        }

        // --- DOMAIN BEHAVIOR ---
        public Result<OrderItem> AddItem(Product product, ProductVariant variant, int quantity, decimal discountPerItem = 0)
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Cancelled)
                return Result.Failure<OrderItem>(Error.Validation("Order.CannotAddItem", $"Cannot add items when order status is '{Status}'."));

            if (quantity <= 0)
                return Result.Failure<OrderItem>(Error.Validation("OrderItem.InvalidQuantity", "Quantity must be greater than zero."));

            if (product.Id != variant.ProductId)
                return Result.Failure<OrderItem>(Error.Validation("OrderItem.VariantMismatch", "The product variant does not belong to the specified product."));

            var existingItem = _orderItems.FirstOrDefault(i => i.ProductVariantId == variant.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
                RecalculateTotals();

                return existingItem;
            }

            var orderItem = new OrderItem(Id, product, variant, quantity, discountPerItem);
            _orderItems.Add(orderItem);

            RecalculateTotals();

            return orderItem;
        }

        public Result ChangeOrdetItemQuantity(Guid orderItemId, int Quantity)
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Cancelled)
                return Result.Failure(Error.Validation("Order.CannotUpdateItem", $"Cannot update items when order status is '{Status}'."));

            var existingItem = _orderItems.FirstOrDefault(i => i.Id == orderItemId);
            if (existingItem is null)
            {
                return Result.Failure(Error.NotFound("OrderItem.NotFound", "Order item not found in the order."));
            }
            existingItem.UpdateQuantity(Quantity);
            RecalculateTotals();

            return Result.Success();
        }

        public Result RemoveOrderItem(Guid orderItemId)
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Cancelled)
                return Result.Failure(Error.Validation("Order.CannotRemoveItem", $"Cannot remove items when order status is '{Status}'."));

            var existingItem = _orderItems.FirstOrDefault(i => i.Id == orderItemId);
            if (existingItem is null)
            {
                return Result.Failure(Error.NotFound("OrderItem.NotFound", "Order item not found in the order."));
            }

            _orderItems.Remove(existingItem);
            RecalculateTotals();

            return Result.Success();
        }

        public Result UpdateFinancialDetails(decimal shippingFee, decimal taxAmount, decimal discountAmount)
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Cancelled)
                return Result.Failure(Error.Validation("Order.CannotUpdateFinancials", $"Cannot update financial details when order status is '{Status}'."));

            if (shippingFee < 0)
                return Result.Failure(Error.Validation("Order.InvalidShippingFee", "Shipping fee must be greater than or equal to zero."));
            if (taxAmount < 0)
                return Result.Failure(Error.Validation("Order.InvalidTaxAmount", "Tax amount must be greater than or equal to zero."));
            if (discountAmount < 0)
                return Result.Failure(Error.Validation("Order.InvalidDiscountAmount", "Discount amount must be greater than or equal to zero."));

            ShippingFee = shippingFee;
            TaxAmount = taxAmount;
            DiscountAmount = discountAmount;
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

        public Result<OrderPayment> AddPayment(decimal amount, DateTime payementDate, PaymentMethod paymentMethod, string? transactionId, string? notes = null)
        {
            if (amount <= 0)
                return Result.Failure<OrderPayment>(Error.Validation("Payment.InvalidAmount", "Payment amount must be greater than zero."));

            //if (amount > AmountDue)
            //    return Result.Failure<OrderPayment>(Error.Validation("Payment.Overpayment", "Payment amount exceeds the amount due."));

            var payment = new OrderPayment(Id, amount, payementDate, paymentMethod, transactionId, notes);
            _payments.Add(payment);

            RecalculatePaymentStatus();

            return payment;
        }

        public Result<OrderPayment> AddRefund(decimal amountToRefund, DateTime payementDate, PaymentMethod paymentMethod, string reason, string? transactionId)
        {
            if (amountToRefund <= 0)
                return Result.Failure<OrderPayment>(Error.Validation("Refund.InvalidAmount", "Refund amount must be positive."));

            if (amountToRefund > AmountPaid)
                return Result.Failure<OrderPayment>(Error.Validation("Refund.Excessive", "Cannot refund more than was paid."));

            var refund = new OrderPayment(
                Id,
                -amountToRefund,
                payementDate,
                paymentMethod,
                transactionId,
                notes: $"Refund: {reason}");

            _payments.Add(refund);

            RecalculatePaymentStatus();

            return Result.Success(refund);
        }

        public Result<OrderPayment> VoidPayment(Guid paymentId)
        {
            var payment = _payments.FirstOrDefault(p => p.Id == paymentId);

            if (payment is null)
                return Result.Failure<OrderPayment>(Error.NotFound("Payment.NotFound", "Payment not found."));

            if (payment.Status != Entities.PaymentStatus.Pending && payment.Provider != "System")
                return Result.Failure<OrderPayment>(Error.Validation("Payment.CannotVoid", "Only pending online payments and system payments can be voided."));

            payment.Void();
            RecalculatePaymentStatus();
            return Result.Success(payment);
        }

        private void RecalculatePaymentStatus()
        {
            AmountPaid = _payments
                .Where(p => p.Status == Entities.PaymentStatus.Completed)
                .Sum(p => p.Amount);

            AmountDue = TotalAmount - AmountPaid;

            if (AmountPaid >= TotalAmount)
            {
                OrderPaymentStatus = AmountPaid > TotalAmount
                    ? OrderPaymentStatus.Overpaid
                    : OrderPaymentStatus.Paid;
            }
            else if (AmountPaid > 0)
            {
                OrderPaymentStatus = OrderPaymentStatus.Partial;
            }
            else
            {
                bool hasRefunds = _payments.Any(p => p.Status == Entities.PaymentStatus.Completed && p.Amount < 0);

                OrderPaymentStatus = hasRefunds
                    ? OrderPaymentStatus.Refunded
                    : OrderPaymentStatus.Unpaid;
            }
        }

        public Result UpdateDeliveryInfo(
            Guid courierId,
            string deliveryAddress,
            string? deliveryCity,
            string? deliveryRegion,
            string? deliveryCountry,
            string? deliveryPostalCode,

            string? trackingNumber,
            string? notes,
            bool isCashOnDelivery)
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Cancelled)
            {
                return Result.Failure(Error.Validation("Order.CannotUpdate", $"Cannot update delivery details when order status is '{Status}'."));
            }

            if (Status >= OrderStatus.Confirmed && IsCashOnDelivery != isCashOnDelivery)
            {
                return Result.Failure(Error.Validation("Order.CannotUpdatePaymentType", "Cannot change Payment Type when order status is Confirmed or later."));
            }

            if (string.IsNullOrWhiteSpace(deliveryAddress))
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
            IsCashOnDelivery = isCashOnDelivery;

            return Result.Success();
        }

        public Result Confirm(List<ProductVariant> currentInventory)
        {
            if (Status != OrderStatus.Pending)
                return Result.Failure(Error.Validation(
                    "Order.CannotConfirm",
                    $"Cannot confirm order when status is '{Status}'."));

            if (!_orderItems.Any())
                return Result.Failure(Error.Validation(
                    "Order.NoItems",
                    "Cannot confirm an order with no items."));

            if (string.IsNullOrWhiteSpace(DeliveryAddress))
                return Result.Failure(Error.Validation(
                    "Order.MissingAddress",
                    "Delivery address is required to confirm the order."));

            if (OrderPaymentStatus == OrderPaymentStatus.Unpaid && !IsCashOnDelivery)
            {
                return Result.Failure(Error.Validation("Order.Unpaid", "Payment required before confirmation."));
            }

            foreach (var item in _orderItems)
            {
                var variant = currentInventory.FirstOrDefault(v => v.Id == item.ProductVariantId);
                if (variant is null)
                    return Result.Failure(Error.NotFound("OrderItem.VariantNotFound", $"Variant for {item.ProductName} not found."));

                if (variant.StockQuantity >= item.Quantity)
                {
                    var result = variant.AdjustStock(-item.Quantity);
                    if (result.IsFailure)
                        return result;
                }
                else
                {
                    return Result.Failure(Error.Conflict(
                        "OrderItem.InsufficientStock",
                        $"Insufficient stock for {item.ProductName} (Requested: {item.Quantity}, Available: {variant.StockQuantity})."));
                }
            }

            Status = OrderStatus.Confirmed;

            return Result.Success();
        }

        public Result StartProcessing()
        {
            if (Status != OrderStatus.Confirmed)
                return Result.Failure(Error.Validation(
                    "Order.InvalidState",
                    $"Order must be in 'Confirmed' state to start processing. Current state: '{Status}'."));

            if (!_orderItems.Any())
                return Result.Failure(Error.Validation(
                    "Order.NoItems",
                    "Cannot process an empty order."));

            Status = OrderStatus.Confirmed;

            return Result.Success();
        }

        public Result MarkAsReadyToShip()
        {
            if (Status != OrderStatus.Confirmed)
                return Result.Failure(Error.Validation(
                    "Order.InvalidState",
                    $"Order must be in 'Confirmed' state to be marked as Ready To Ship. Current state: '{Status}'."));

            if (!_orderItems.Any())
                return Result.Failure(Error.Validation("Order.NoItems", "Cannot pack an empty order."));

            if (CourierId is null || CourierId == Guid.Empty)
                return Result.Failure(Error.Validation(
                    "Order.MissingCourier",
                    "Courier must be assigned before marking the order as Ready To Ship."));

            Status = OrderStatus.Packed;

            return Result.Success();
        }

        public Result MarkAsShipped()
        {
            if (Status != OrderStatus.Packed)
                return Result.Failure(Error.Validation(
                    "Order.InvalidState",
                    $"Order must be in 'Packed' state to be marked as Shipped. Current state: '{Status}'."));

            if (string.IsNullOrWhiteSpace(TrackingNumber))
                return Result.Failure(Error.Validation(
                    "Order.MissingTrackingNumber",
                    "Tracking number is required to mark the order as shipped."));

            Status = OrderStatus.Shipped;
            return Result.Success();
        }

        public Result MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
                return Result.Failure(Error.Validation(
                    "Order.InvalidState",
                    $"Order must be in 'Shipped' state to be marked as Delivered. Current state: '{Status}'."));

            Status = OrderStatus.Delivered;
            return Result.Success();
        }

        public Result Cancel(string reason, bool returnToStock, List<ProductVariant> inventorySnapshot)
        {
            if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
                return Result.Failure(Error.Validation(
                    "Order.TooLate",
                    "Cannot cancel an order that has already been shipped. Use the Return workflow instead."));

            if (Status == OrderStatus.Cancelled)
                return Result.Failure(Error.Validation("Order.AlreadyCancelled", "Order is already cancelled."));

            Status = OrderStatus.Cancelled;

            if(returnToStock && Status >= OrderStatus.Confirmed)
            {
                foreach(var item in _orderItems)
                {
                    var variant = inventorySnapshot.FirstOrDefault(v => v.Id == item.ProductVariantId);

                    // It's possible the variant was deleted from DB, so we check null
                    if (variant != null)
                    {
                        variant.AdjustStock(item.Quantity);
                    }
                }
            }

            Status = OrderStatus.Cancelled;

            Notes = string.IsNullOrWhiteSpace(Notes)
                ? $"Cancelled: {reason}"
                : $"{Notes} | Cancelled: {reason}";

            return Result.Success();
        }
    }
}
