using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.AddProductImage
{
    public class AddProductImageHandler : ICommandHandler<AddProductImageCommand>
    {
        private readonly AppDbContext _dbContext;

        public AddProductImageHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(AddProductImageCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

            if(product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            var result = product.AddImage(command.ImageUrl, command.IsPrimary);
            if(result.IsFailure)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
