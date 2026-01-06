using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.DeactivateProduct
{
    public class DeactivateProductHandler : ICommandHandler<DeactivateProductCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeactivateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeactivateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            product.Deactivate();

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
