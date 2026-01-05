using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Features.Catalog.Entities
{
    public sealed class ProductImage : Entity
    {
        private ProductImage() { }

        internal ProductImage(Guid productId, string imageUrl, int displayOrder, bool isPrimary) 
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ImageUrl = imageUrl;
            DiaplayOrder = displayOrder;
            IsPrimary = isPrimary;
        }

        public Guid ProductId { get; private set; }
        public string ImageUrl { get; private set; } = string.Empty;
        public int DiaplayOrder { get; private set; }
        public bool IsPrimary { get; private set; }

        internal void SetPrimary(bool isPrimary) => IsPrimary = isPrimary;
    }
}
