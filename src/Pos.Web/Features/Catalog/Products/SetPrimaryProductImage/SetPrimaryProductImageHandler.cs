using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Catalog.Products.SetPrimaryProductImage
{
    public class SetPrimaryProductImageHandler : ICommandHandler<SetPrimaryProductImageCommand>
    {
        private readonly AppDbContext _dbCOntext;

        public SetPrimaryProductImageHandler(AppDbContext dbCOntext)
        {
            _dbCOntext = dbCOntext;
        }

        public async Task<Result> Handle(SetPrimaryProductImageCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbCOntext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

            if (product is null)
                return Result.Failure(Error.NotFound("Product.NotFound", "Product not found."));

            var result = product.SetPrimaryImage(command.ImageId);
            if (result.IsFailure)
                return result;

            await _dbCOntext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
