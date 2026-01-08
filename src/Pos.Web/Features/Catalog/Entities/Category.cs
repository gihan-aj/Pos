using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Entities
{
    public class Category : AuditableEntity
    {
        public const int MaxDepth = 2; // 0=Root, 1=Sub 2=SubSub
        public const char PathSeparator = '/';
        public const string NameSeparator = " / ";

        private Category()
        {
        }
        public Category(
            Guid id,
            string name, 
            int level,
            string path,
            string namePath,
            string? description = null, 
            Guid? parentCategoryId = null,
            int displayOrder = 0,
            string? iconUrl = null, 
            string? color = null)
        {
            Id = id;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            DisplayOrder = displayOrder;
            IconUrl = iconUrl;
            Color = color;
            Level = level;
            Path = path;
            NamePath = namePath;
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

        // Hierarchy management
        public int Level { get; private set; } // 0 for root

        // Materialized Path: /RootId/ChildId/GrandChildId
        public string Path { get; private set; } = string.Empty;

        // Breadcrumb: "Root / Child / GrandChild"
        public string NamePath { get; private set; } = string.Empty;

        // Navigation properties
        public Category? ParentCategory { get; private set; }

        private readonly List<Category> _subCategories = new();
        public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

        // --- Factory Method ---
        public static Result<Category> Create(
            string name,
            string? description = null,
            Category? parent = null,
            int displayOrder = 0,
            string? iconUrl = null,
            string? color = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Category>(Error.Validation("Category.NameRequired", "Category name is required."));

            int level = 0;
            Guid? parentId = null;
            string parentPath = string.Empty;
            string parentNamePath = string.Empty;

            if (parent is not null)
            {
                if(parent.Level >= MaxDepth)
                {
                    return Result.Failure<Category>(Error.Validation("Category.MaxDepth", $"Maximum category depth of {MaxDepth + 1} levels reached."));
                }

                level = parent.Level + 1;
                parentId = parent.Id;
                parentPath = parent.Path;
                parentNamePath = parent.NamePath;
            }

            var id = Guid.NewGuid();

            // Generate path
            var path = string.IsNullOrEmpty(parentPath)
                ? $"{PathSeparator}{id}{PathSeparator}"
                : $"{parentPath}{id}{PathSeparator}";

            var namePath = string.IsNullOrEmpty(parentPath)
                ? name
                : $"{parentNamePath}{NameSeparator}{name}";

            var category = new Category(id, name, level, path, namePath, description, parentId, displayOrder, iconUrl, color);

            // category.RaiseDomainEvent(new CategoryCreatedEvent(category.Id));

            return Result.Success(category);
        }

        // --- Behaviors ---
        public void UpdateDetails(string name, string? description, int displayOrder, string? iconUrl, string? color)
        {
            if(Name != name)
            {
                if (!string.IsNullOrEmpty(NamePath))
                {
                    if (NamePath.EndsWith(Name))
                    {
                        NamePath = NamePath.Substring(0, NamePath.Length - Name.Length) + name;
                    }
                }
                else
                {
                    NamePath = name;
                }
            }

            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
            IconUrl = iconUrl;
            Color = color;
        }

        public Result ChangeParent(Category? newParent)
        {
            // Direct circular ref
            if(newParent?.Id == Id)
            {
                return Result.Failure(Error.Conflict("Category.Circular", "Cannot map category to itself."));
            }

            // Deep circular ref
            if(newParent is not null && newParent.Path.Contains($"{PathSeparator}{Id}{PathSeparator}"))
            {
                return Result.Failure(Error.Conflict("Category.CircularDeep", "Cannot move a category into its own child."));
            }

            // Check path
            int newLevel = newParent?.Level + 1 ?? 0;
            if(newLevel > MaxDepth)
            {
                return Result<Category>.Failure(Error.Validation("Category.MaxDepth", "Moving here exceeds maximum depth."));
            }

            ParentCategoryId = newParent?.Id;
            Level = newLevel;

            var newParentPath = newParent?.Path ?? string.Empty;
            Path = string.IsNullOrEmpty(newParentPath)
                ? $"{PathSeparator}{Id}{PathSeparator}"
                : $"{newParentPath}{Id}{PathSeparator}";

            var newParentNamePath = newParent?.NamePath ?? string.Empty;
            NamePath = string.IsNullOrEmpty(newParentNamePath)
                ? $"{Name}"
                : $"{newParentNamePath}{NameSeparator}{Name}";

            // The Handler MUST load these children for this to work
            foreach (var child in _subCategories)
            {
                var updatedResult = child.UpdatePathFromParent(Path, NamePath, Level);
                if (updatedResult.IsFailure) return updatedResult;
            }

            return Result.Success();
        }

        internal Result UpdatePathFromParent(string parentPath, string parentNamePath, int parentLevel)
        {
            var newLevel = parentLevel + 1;
            if (newLevel > MaxDepth)
            {
                return Result.Failure(Error.Validation("Category.MaxDepth", $"A sub-category exceeds the maximum depth of {MaxDepth + 1} levels."));
            }

            Path = $"{parentPath}{Id}{PathSeparator}";
            NamePath = $"{parentNamePath}{NameSeparator}{Name}";
            Level = newLevel;

            foreach (var child in _subCategories)
            {
                var result = child.UpdatePathFromParent(Path, NamePath, Level);
                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Result.Success(this);
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
