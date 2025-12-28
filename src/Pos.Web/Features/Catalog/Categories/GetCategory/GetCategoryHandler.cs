using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Categories.GetCategory
{
    public class GetCategoryHandler
    {
        private readonly AppDbContext _dbContext;

        public GetCategoryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery query, CancellationToken cancellationToken)
        {
            var includeSub = query.Includes?.Contains("subcategories", StringComparer.OrdinalIgnoreCase) ?? false;
            var includeProd = query.Includes?.Contains("products", StringComparer.OrdinalIgnoreCase) ?? false;

            // AsNoTracking for read performance since we aren't modifying these entities
            var dbQuery = _dbContext.Categories.AsNoTracking().AsQueryable();

            if (includeSub)
            {
                dbQuery = dbQuery.Include(c => c.SubCategories);
            }

            var category = await dbQuery.FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);
            if (category is null)
                return Result.Failure<CategoryResponse>(Error.NotFound("Category.NotFound", "Category not found"));

            // Handle Products (Separate Query to keep Domain clean)
            // Since 'Products' isn't a navigation property on Category (in our strict Write model), 
            // we fetch them manually if requested. This is often cleaner for VSA.
            List<ProductSummary>? products = null;
            if (includeProd)
            {
                // Assuming Product entity exists and has CategoryId
                // Using a projection to fetch only what we need
                // products = await _dbContext.Products
                //    .AsNoTracking()
                //    .Where(p => p.CategoryId == query.Id)
                //    .Select(p => new ProductSummary(p.Id, p.Name, p.Sku, p.Price)) // Assuming Price exists on Product or Variant
                //    .ToListAsync(ct);

                // Mocking for now as Product entity details might vary
                products = new List<ProductSummary>();
            }

            var response = new CategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.ParentCategoryId,
                category.IsActive,
                category.DisplayOrder,
                category.IconUrl,
                category.Color,
                SubCategories: includeSub
                    ? category.SubCategories.Select(MapToResponse).ToList()
                    : null,
                Products: products
            );

            return Result.Success(response);
        }

        private static CategoryResponse MapToResponse(Category c) =>
            new(c.Id, c.Name, c.Description, c.ParentCategoryId, c.IsActive, c.DisplayOrder, c.IconUrl, c.Color, null, null);
    }
}
