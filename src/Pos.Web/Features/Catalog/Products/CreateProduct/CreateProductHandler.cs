using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.CreateProduct
{
    public class CreateProductHandler : ICommandHandler<CreateProductCommand, Guid>
    {
        private readonly AppDbContext _dbContext;

        public CreateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var categoryInfo = await _dbContext.Categories
                .Where(c => c.Id == command.CategoryId)
                .Select(c => new { c.Id, HasChildren = c.SubCategories.Any() })
                .FirstOrDefaultAsync(cancellationToken);

            if (categoryInfo is null)
                return Result.Failure<Guid>(Error.NotFound("Category.NotFound", "Category not found or not active."));

            if (categoryInfo.HasChildren)
                return Result.Failure<Guid>(Error.Conflict("Category.NotLeaf", "Products can only be assigned to leaf categories (categories with no sub-categories)."));

            string finalSku;
            if (!string.IsNullOrWhiteSpace(command.SkuOverride))
            {
                bool skuExists = await _dbContext.Products.AnyAsync(p => p.Sku == command.SkuOverride, cancellationToken);
                if (skuExists)
                {
                    return Result.Failure<Guid>(Error.Conflict("Product.DuplicateSku", $"SKU '{command.SkuOverride}' is already taken."));
                }

                finalSku = command.SkuOverride;
            }
            else
            {
                // Auto-Generate: PROD-1001, PROD-1002, etc.
                finalSku = await GenerateNextSkuAsync(cancellationToken);
            }

            var productResult = Product.Create(
                command.Name,
                command.Description,
                command.CategoryId,
                finalSku,
                command.Brand,
                command.Material,
                command.Gender,
                command.BasePrice,
                command.Tags
            );

            if (productResult.IsFailure) return Result.Failure<Guid>(productResult.Error);

            _dbContext.Products.Add(productResult.Value);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return productResult.Value.Id;
        }

        private async Task<string> GenerateNextSkuAsync(CancellationToken cancellationToken) 
        {
            // Strategy: Find the highest existing auto-generated SKU and increment.
            // Format: "PROD-XXXX"

            // OPTION 1: Sequence Table (Best for concurrency)
            // OPTION 2: Max Query (Simpler for MVP, potential race condition in high-concurrency)

            // We'll use a safer "Retry" approach or just Max for this scale.
            // NOTE: In high traffic, use a dedicated DB Sequence or Distributed Counter (Redis).

            var lastSku = await _dbContext.Products
                .Where(p => p.Sku != null && p.Sku.StartsWith("PROD-"))
                .OrderByDescending(p => p.Sku) // String sort works for fixed length, but tricky for "PROD-9" vs "PROD-10"
                .Select(p => p.Sku)
                .FirstOrDefaultAsync(cancellationToken);

            int nextNumber = 1000; // Start at 1000
            if(lastSku != null && lastSku.Length > 5)
            {
                if(int.TryParse(lastSku.Substring(5), out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            return $"PROD-{nextNumber}";
        }

    }

}
