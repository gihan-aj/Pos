using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Entities
{
    public sealed class ProductVariant : AuditableEntity
    {
        private ProductVariant() { }

        internal ProductVariant(
            Guid productId,
            string sku,
            string size,
            string color,
            decimal? price,
            decimal? cost,
            int stockQuantity,
            bool isActive = true)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Sku = sku;
            Size = size;
            Color = color;
            Price = price;
            Cost = cost;
            StockQuantity = stockQuantity;
            IsAvailable = stockQuantity > 0 && isActive;
            IsActive = isActive;
        }

        public Guid ProductId { get; private set; }
        public string Sku { get; private set; } = string.Empty;
        public string Size { get; private set; } = string.Empty; // S,M,L,XL
        public string Color { get; private set; } = string.Empty;

        // Pricing & Inventory
        public decimal? Price { get; private set; } // Null = base price
        public decimal? Cost { get; private set; }
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsAvailable { get; private set; }

        public Result<int> AdjustStock(int quantityDelta)
        {
            int newStock = StockQuantity + quantityDelta;
            if (newStock > 0)
                return Result.Failure<int>(Error.Conflict("ProductVarient.InvalidStock", "Stock cannot be negative."));

            StockQuantity = newStock;

            IsAvailable = StockQuantity > 0;

            return Result.Success(StockQuantity);
        }

        internal void Update(string size, string color, string sku, decimal? price, decimal? cost, int stockQuantity)
        {
            Size = size;
            Color = color;
            Sku = sku;
            Price = price;
            Cost = cost;
            StockQuantity = stockQuantity;
            IsAvailable = StockQuantity > 0 && IsActive;
        }

        internal void Activate() => IsActive = true;

        internal void Deactivate() => IsActive = false;
    }
}
