namespace Pos.Web.Shared.Dtos
{
    public class InvoiceItemModel
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
