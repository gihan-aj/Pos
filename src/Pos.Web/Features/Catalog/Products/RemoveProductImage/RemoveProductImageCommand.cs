using Pos.Web.Shared.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pos.Web.Features.Catalog.Products.RemoveProductImage
{
    public record RemoveProductImageCommand(Guid ProductId, Guid ImageId) : ICommand;
}
