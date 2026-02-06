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
        private readonly IAppSequenceService _appSequenceService;

        public CreateProductHandler(AppDbContext dbContext, IAppSequenceService appSequenceService)
        {
            _dbContext = dbContext;
            _appSequenceService = appSequenceService;
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
            if (!string.IsNullOrWhiteSpace(command.Sku))
            {
                bool skuExists = await _dbContext.Products.AnyAsync(p => p.Sku == command.Sku, cancellationToken);
                if (skuExists)
                {
                    return Result.Failure<Guid>(Error.Conflict("Product.DuplicateSku", $"SKU '{command.Sku}' is already taken."));
                }

                finalSku = command.Sku;
            }
            else
            {
                // Auto-Generate: PROD-1001, PROD-1002, etc.
                //finalSku = await GenerateNextSkuAsync(cancellationToken);
                finalSku = await _appSequenceService.GetNextNumberAsync("Sku", cancellationToken);
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

            var product = productResult.Value;

            _dbContext.Products.Add(product);

            // Variants
            foreach(var variant in command.Variants)
            {
                string variantSku;
                if (!string.IsNullOrWhiteSpace(variant.Sku))
                {
                    bool skuExists = await _dbContext.Set<ProductVariant>().AnyAsync(v => v.Sku == variant.Sku, cancellationToken);
                    if (skuExists)
                        return Result.Failure<Guid>(Error.Conflict("Variant.DuplicateSku", $"SKU '{variant.Sku}' is already in use."));

                    variantSku = variant.Sku;
                }
                else
                {
                    if (string.IsNullOrEmpty(product.Sku))
                        return Result.Failure<Guid>(Error.Conflict("Product.NoSku", "Cannot auto-generate variant SKU because parent product has no SKU. Please provide a manual SKU."));

                    string colorCode = GenerateCode(variant.Color, 3);
                    string sizeCode = GenerateCode(variant.Size, 4);

                    variantSku = $"{product.Sku}-{colorCode}-{sizeCode}";
                }

                var result = product.AddVarient(
                    variant.Size,
                    variant.Color,
                    variantSku,
                    variant.Price,
                    variant.Cost,
                    variant.StockQuantity);

                if (result.IsFailure)
                {
                    return Result.Failure<Guid>(result.Error);
                }
            }

            // Images
            foreach(var image in command.Images)
            {
                var result = product.AddImage(image.ImageUrl, image.IsPrimary);
                if (result.IsFailure)
                    return Result.Failure<Guid>(result.Error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return productResult.Value.Id;
        }



        private static string GenerateCode(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input)) return "XXX";

            var cleaned = input.Replace(" ", "").ToUpperInvariant();
            return cleaned.Length <= length ? cleaned : cleaned.Substring(0, length);
        }
    }

}
