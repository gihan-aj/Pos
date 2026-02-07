using Pos.Web.Shared.Dtos;

namespace Pos.Web.Shared.Abstractions
{
    public interface IInvoiceGenerator
    {
        byte[] Generate(InvoiceModel model);
    }
}
