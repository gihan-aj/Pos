using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.UpdateProduct
{
    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            if(product.CategoryId != command.CategoryId)
            {
                var categoryInfo = await _dbContext.Categories
                    .Where(c => c.Id == command.CategoryId)
                    .Select(c => new { c.Id, HasChildren = c.SubCategories.Any()})
                    .FirstOrDefaultAsync(cancellationToken);

                if (categoryInfo is null)
                    return Result.Failure(Error.NotFound("Category.NotFound", "Category not found."));

                if (categoryInfo.HasChildren)
                    return Result.Failure<Guid>(Error.Conflict("Category.NotLeaf", "Products can only be assigned to leaf categories (categories with no sub-categories)."));
            }

            product.UpdateDetails(
                command.Name,
                command.Description,
                command.CategoryId,
                command.Brand,
                command.Material,
                command.Gender,
                command.BasePrice,
                command.Tags ?? new List<string>()
            );

            var modifiedVariantIds = command.Variants
                .Where(v => v.Id != Guid.Empty)
                .Select(v => v.Id)
                .ToHashSet();

            var existingVariantIds = product.Variants
                .Select(v => v.Id)
                .ToHashSet();

            var deletedVariantIds = existingVariantIds.Except(modifiedVariantIds);
            foreach( var variantId in deletedVariantIds)
            {
                var removeVariantResult = product.RemoveVariant(variantId);
                if (removeVariantResult.IsFailure)
                    return removeVariantResult;
            }
            
            foreach(var variant in command.Variants)
            {
                if(variant.Id != Guid.Empty)
                {
                    var updateVariantResult = product.UpdateVariant(
                        variant.Id,
                        variant.Size,
                        variant.Color,
                        variant.Sku,
                        variant.Price,
                        variant.Cost,
                        variant.StockQuantity);

                    if(updateVariantResult.IsFailure)
                        return updateVariantResult;
                    
                }
                else
                {
                    string variantSku;
                    if (!string.IsNullOrWhiteSpace(variant.Sku))
                    {
                        // Check global uniqueness for manual override (Database Check)
                        // Note: The Entity check covers siblings, but for manual override we should check global uniqueness to be safe.
                        // However, the DB Unique Index will catch this too. For user friendliness, we can check.
                        bool skuExists = await _dbContext.Set<ProductVariant>().AnyAsync(v => v.Sku == variant.Sku, cancellationToken);
                        if (skuExists)
                            return Result.Failure(Error.Conflict("Variant.DuplicateSku", $"SKU '{variant.Sku}' is already in use."));

                        variantSku = variant.Sku;
                    }
                    else
                    {
                        // Auto-Generate Smart SKU
                        // Format: {ParentSKU}-{ColorCode}-{SizeCode}
                        // Example: PROD-1001-BLU-XL
                        if (string.IsNullOrEmpty(product.Sku))
                            return Result.Failure(Error.Conflict("Product.NoSku", "Cannot auto-generate variant SKU because parent product has no SKU. Please provide a manual SKU."));

                        string colorCode = GenerateCode(variant.Color, 3);
                        string sizeCode = GenerateCode(variant.Size, 4);

                        variantSku = $"{product.Sku}-{colorCode}-{sizeCode}";

                        // Have to heck if this generated SKU happens to collide (e.g., "Blue" vs "Blur" both -> "BLU")
                        // The Entity.AddVariant method will catch duplicate SKUs within the product,
                        // but we might want to append a number if it collides globally or locally? 
                        // For now, we assume the user names colors distinctly enough or creates unique combos.
                    }

                    var addVariantResult = product.AddVarient(
                        variant.Size,
                        variant.Color,
                        variantSku,
                        variant.Price,
                        variant.Cost,
                        variant.StockQuantity);

                    if(addVariantResult.IsFailure)
                        return addVariantResult;
                }
            }

            var unchangedImageIds = command.Images
                .Where(i => i.Id != Guid.Empty)
                .Select(i => i.Id)
                .ToHashSet();

            var existingImageIds = product.Images
                .Select(i => i.Id)
                .ToHashSet();

            var deletedImageIds = existingImageIds.Except(unchangedImageIds);
            foreach( var imageId in deletedImageIds)
            {
                var removeImageResult = product.RemoveImage(imageId);
                if(removeImageResult.IsFailure)
                    return removeImageResult;
            }

            var newImages = command.Images
                .Where(i => i.Id == Guid.Empty);
            foreach( var image in newImages)
            {
                var addImageResult = product.AddImage(image.ImageUrl, image.IsPrimary);
                if(addImageResult.IsFailure)
                    return addImageResult;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private static string GenerateCode(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input)) return "XXX";

            var cleaned = input.Replace(" ", "").ToUpperInvariant();
            return cleaned.Length <= length ? cleaned : cleaned.Substring(0, length);
        }
    }
}
