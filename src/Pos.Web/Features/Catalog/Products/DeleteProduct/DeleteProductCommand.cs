using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Shared.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pos.Web.Features.Catalog.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand;
}
