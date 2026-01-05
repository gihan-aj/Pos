using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Entities
{
    public sealed class Product : AuditableEntity
    {
        private Product() { }

        private Product(
            string name,
            string? description,
            Guid categoryId,
            string? sku,
            string? brand,
            string? material,
            Gender? gender,
            decimal basePrice,
            List<string> tags)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Sku = sku;
            Brand = brand;
            Material = material;
            Gender = gender;
            BasePrice = basePrice;
            Tags = tags;
            IsActive = true;
        }

        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public Guid CategoryId { get; private set; }

        // Optiona Base SKU
        public string? Sku {  get; private set; }

        // Clothing Specifics
        public string? Brand { get; private set; }

        public string? Material { get; private set; }

        public Gender? Gender { get; private set; }

        // Stored as JSON 
        public List<string> Tags { get; private set; } = new();


        // Pricing
        public decimal BasePrice { get; private set; }

        public bool IsActive { get; private set; }

        // Navigation
        public Category? Category { get; private set; }

        private readonly List<ProductVariant> _varients = new();
        public IReadOnlyCollection<ProductVariant> Varients => _varients.AsReadOnly();

        private readonly List<ProductImage> _images = new();
        public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

        // -- Factory --
        public static Result<Product> Create(
            string name,
            string? description,
            Guid categoryId,
            string? sku,
            string? brand,
            string? material,
            Gender? gender,
            decimal basePrice,
            List<string>? tags)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Product>(Error.Validation("Product.NameRequired", "Product name is required."));

            if (basePrice < 0)
                return Result.Failure<Product>(Error.Validation("Product.InvalidPrice", "Base price cannot be negative."));

            var product = new Product(
                name,
                description,
                categoryId,
                sku,
                brand,
                material,
                gender,
                basePrice,
                tags ?? new List<string>());

            return Result.Success(product);
        }

        // --- Behaviors ---
        public Result<ProductVariant> AddVarient(
            string size, 
            string color, 
            string sku, 
            decimal? priceOverride = null, 
            decimal? cost = null, 
            int stockQuantity = 0)
        {
            if(_varients.Any(v => v.Sku == sku))
                return Result.Failure<ProductVariant>(Error.Conflict("Variant.DuplicateSku", $"Variant with SKU '{sku}' already exists."));

            if(_varients.Any(v => v.Size == size && v.Color == color))
                return Result.Failure<ProductVariant>(Error.Conflict("Variant.DuplicateCombo", $"Variant with Size '{size}' and Color '{color}' already exists."));

            var varient = new ProductVariant(Id, sku, size, color, priceOverride, cost, stockQuantity);
            _varients.Add(varient);

            return varient;
        }

        public Result AddImage(string imageUrl, bool isPrimary)
        {
            if (isPrimary)
            {
                foreach (var img in _images) img.SetPrimary(false);
            }

            // If this is the first image, force it to be primary
            if (_images.Count == 0) isPrimary = true;

            var image = new ProductImage(Id, imageUrl, _images.Count + 1, isPrimary);
            _images.Add(image);

            return Result.Success();
        }

        public Result RemoveImage(Guid imageId)
        {
            var image = _images.FirstOrDefault(img => img.Id == imageId);
            if (image is null)
                return Result.Failure(Error.NotFound("Image.NotFound", "Image not found."));

            _images.Remove(image);

            if(image.IsPrimary && _images.Count > 0)
            {
                _images[0].SetPrimary(true);
            }

            return Result.Success();
        }

        public Result SetPrimaryImage(Guid imageId)
        {
            var image = _images.FirstOrDefault(img => img.Id == imageId);
            if (image is null)
                return Result.Failure(Error.NotFound("Image.NotFound", "Image not found."));

            if(image.IsPrimary) 
                return Result.Success();

            foreach(var img in _images) img.SetPrimary(false);

            image.SetPrimary(true);

            return Result.Success();
        }
    }
}
