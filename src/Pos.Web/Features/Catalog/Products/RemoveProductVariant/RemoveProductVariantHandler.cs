using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.RemoveProductVariant
{
    public class RemoveProductVariantHandler : ICommandHandler<RemoveProductVariantCommand>
    {
        private readonly AppDbContext _dbContext;

        public RemoveProductVariantHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(RemoveProductVariantCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.Varients)
                .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);
            if(product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            var result = product.RemoveVariant(command.VariantId);
            if(result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
