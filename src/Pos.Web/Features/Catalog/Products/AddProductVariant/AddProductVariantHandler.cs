using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.AddProductVariant
{
    public class AddProductVariantHandler : ICommandHandler<AddProductVariantCommand>
    {
        private readonly AppDbContext _dbContext;

        public AddProductVariantHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(AddProductVariantCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.Varients)
                .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

            if(product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            string variantSku;
            if (!string.IsNullOrWhiteSpace(command.SkuOverride))
            {
                // Check global uniqueness for manual override (Database Check)
                // Note: The Entity check covers siblings, but for manual override we should check global uniqueness to be safe.
                // However, the DB Unique Index will catch this too. For user friendliness, we can check.
                bool skuExists = await _dbContext.Set<ProductVariant>().AnyAsync(v => v.Sku == command.SkuOverride, cancellationToken);
                if(skuExists)
                    return Result.Failure(Error.Conflict("Variant.DuplicateSku", $"SKU '{command.SkuOverride}' is already in use."));

                variantSku = command.SkuOverride;
            }
            else
            {
                // Auto-Generate Smart SKU
                // Format: {ParentSKU}-{ColorCode}-{SizeCode}
                // Example: PROD-1001-BLU-XL
                if(string.IsNullOrEmpty(product.Sku))
                    return Result.Failure(Error.Conflict("Product.NoSku", "Cannot auto-generate variant SKU because parent product has no SKU. Please provide a manual SKU."));

                string colorCode = GenerateCode(command.Color, 3);
                string sizeCode = GenerateCode(command.Size, 4);

                variantSku = $"{product.Sku}-{colorCode}-{sizeCode}";

                // Have to heck if this generated SKU happens to collide (e.g., "Blue" vs "Blur" both -> "BLU")
                // The Entity.AddVariant method will catch duplicate SKUs within the product,
                // but we might want to append a number if it collides globally or locally? 
                // For now, we assume the user names colors distinctly enough or creates unique combos.
            }

            var result = product.AddVarient(
                command.Size,
                command.Color,
                variantSku,
                command.Price,
                command.Cost,
                command.StockQuantity);

            if (result.IsFailure)
            {
                return result;
            }

            //_dbContext.Set<ProductVariant>().Add(result.Value);

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
