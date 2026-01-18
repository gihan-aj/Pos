using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Features.Customers;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.Entities
{
    public sealed class Order : AuditableEntity
    {
        private Order() { }

        private Order(
            Guid customerId,
            string orderNumber,
            string? deliveryAddress,
            string? deliveryCity,
            string? deliveryPostalCode,
            string? notes,
            decimal shippingFee = 0)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            OrderNumber = orderNumber;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            PaymentStatus = PaymentStatus.Unpaid;
            DeliveryAddress = deliveryAddress;
            DeliveryCity = deliveryCity;
            DeliveryPostalCode = deliveryPostalCode;
            Notes = notes;
            ShippingFee = shippingFee;
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
        public string? DeliveryAddress { get; private set; }
        public string? DeliveryCity { get; private set; }
        public string? DeliveryPostalCode { get; private set; }

        public string? CourierService { get; private set; }
        public string? TrackingNumber { get; private set; }
        public string? Notes { get; private set; }

        // -- Items --
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // --- FACTORY METHOD ---
        public static Result<Order> Create(
            Guid customerId,
            string orderNumber,
            string? deliveryAddress,
            string? deliveryCity,
            string? deliveryPostalCode,
            string? notes,
            decimal shippingFee = 0)
        {
            if(customerId == Guid.Empty)
                return Result.Failure<Order>(new Shared.Errors.Error("Order.InvalidCustomerId", "Customer ID cannot be empty.", Shared.Errors.ErrorType.Validation));

            if(string.IsNullOrEmpty(orderNumber))
                return Result.Failure<Order>(new Shared.Errors.Error("Order.InvalidOrderNumber", "Order number cannot be empty.", Shared.Errors.ErrorType.Validation));

            return new Order(
                customerId,
                orderNumber,
                deliveryAddress,
                deliveryCity,
                deliveryPostalCode,
                notes,
                shippingFee);
        }

        // --- DOMAIN BEHAVIOR ---
        public Result AddItem(Product product, ProductVariant variant, int quantity, decimal discountPerItem = 0)
        {
            if(quantity <= 0)
                return Result.Failure(new Shared.Errors.Error("OrderItem.InvalidQuantity", "Quantity must be greater than zero.", Shared.Errors.ErrorType.Validation));

            if(product.Id != variant.ProductId)
                return Result.Failure(new Shared.Errors.Error("OrderItem.VariantMismatch", "The product variant does not belong to the specified product.", Shared.Errors.ErrorType.Validation));

            var orderItem = new OrderItem(Id, product, variant, quantity, discountPerItem);

            _orderItems.Add(orderItem);

            RecalculateTotals();

            return Result.Success();
        }

        public void RecalculateTotals()
        {
            SubTotal = _orderItems.Sum(oi => oi.SubTotal);
            TotalAmount = SubTotal - DiscountAmount + TaxAmount + ShippingFee;
            AmountDue = TotalAmount - AmountPaid;
        }
    }
}
