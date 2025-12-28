namespace Pos.Web.Features.Catalog.Categories.GetCategory
{
    public record CategoryResponse(
        Guid Id,
        string Name,
        string? Description,
        Guid? ParentCategoryId,
        bool IsActive,
        int DisplayOrder,
        string? IconUrl,
        string? Color,
        List<CategoryResponse>? SubCategories = null,
        List<ProductSummary>? Products = null);
}
