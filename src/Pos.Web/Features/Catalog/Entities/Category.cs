using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Entities
{
    public class Category : AuditableEntity
    {
        private Category()
        {
        }
        public Category(string name, string? description = null, Guid? parentCategoryId = null,
                        int displayOrder = 0,
                        string? iconUrl = null, string? color = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            DisplayOrder = displayOrder;
            IconUrl = iconUrl;
            Color = color;
            IsActive = true;
        }

        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid? ParentCategoryId { get; private set; }
        public bool IsActive { get; private set; }
        public int DisplayOrder { get; private set; }

        // Metadata properties flattened for simplicity, or could be a ValueObject
        public string? IconUrl { get; private set; }
        public string? Color { get; private set; }

        // Navigation properties
        public Category? ParentCategory { get; private set; }

        private readonly List<Category> _subCategories = new();
        public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

        // --- Factory Method ---
        public static Result<Category> Create(
            string name,
            string? description = null,
            Guid? parentCategoryId = null,
            int displayOrder = 0,
            string? iconUrl = null,
            string? color = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Category>(Error.Validation("Category.NameRequired", "Category name is required."));

            var category = new Category(name, description, parentCategoryId, displayOrder, iconUrl, color);

            // We could raise a domain event here if needed
            // category.RaiseDomainEvent(new CategoryCreatedEvent(category.Id));

            return Result.Success(category);
        }

        // --- Behaviors ---
        public void UpdateDetails(string name, string? description, string? iconUrl, string? color)
        {
            // Simple validations can go here or in a Validator before calling this
            Name = name;
            Description = description;
            IconUrl = iconUrl;
            Color = color;
        }

        public Result ChangeParent(Guid? newParentId)
        {
            if(newParentId == Id)
            {
                return Result.Failure(Error.Conflict("Category.CircularParent", "A category cannot be its own parent."));
            }

            // Deeper circular checks (A -> B -> A) usually require a Domain Service 
            // or passing the full hierarchy here, but for now, we block the immediate self-reference.

            ParentCategoryId = newParentId;
            return Result.Success();
        }

        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this category and recursively deactivates all sub-categories.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;

            // Business Rule: "deactivate child categories too"
            // This relies on SubCategories being loaded (Include(x => x.SubCategories))
            foreach (var child in _subCategories)
            {
                child.Deactivate();
            }
        }
    }
}
