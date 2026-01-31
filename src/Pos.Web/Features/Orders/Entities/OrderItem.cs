using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Orders.Entities
{
    public sealed class OrderItem : AuditableEntity
    {
        private OrderItem() { }

        public OrderItem(Guid orderId, Product product, ProductVariant variant, int quantity, decimal discountAmount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductVariantId = variant.Id;

            // Snapshot
            ProductName = product.Name;
            Sku = variant.Sku;
            VariantDetails = $"{variant.Color} {variant.Size}".Trim();

            Quantity = quantity;

            // Money values at the moment of sale
            UnitPrice = variant.Price ?? product.BasePrice;
            UnitCost = variant.Cost;
            DiscountAmount = discountAmount;

            CalculateSubTotal();
        }

        public Guid OrderId { get; private set; }
        public Guid ProductVariantId { get; private set; }
        public ProductVariant? ProductVariant { get; private set; }

        // Snapshot data
        public string ProductName { get; private set; } = string.Empty;
        public string? VariantDetails { get; private set; } // e.g. "Blue, Medium"
        public string Sku { get; private set; } =  string.Empty;
        public int Quantity { get; private set; } 
        public decimal UnitPrice { get; private set; } // Price at moment of sale
        public decimal? UnitCost { get; private set; } // Cost at the moment of sale
        public decimal? DiscountAmount { get; private set; } // Discount per item

        public decimal SubTotal { get; private set; } // (Qty * Price) - Discount

        public void CalculateSubTotal()
        {
            SubTotal = (Quantity * UnitPrice) - (DiscountAmount ?? 0);
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
            CalculateSubTotal();
        }
        
        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
            CalculateSubTotal();
        }
    }
}
