namespace Pos.Web.Shared.Dtos
{
    public class InvoiceModel
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        // Sender (Your Company)
        public AddressModel? SellerAddress { get; set; }

        // Receiver (Customer)
        public AddressModel? CustomerAddress { get; set; }

        public List<InvoiceItemModel> Items { get; set; } = new();

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string PaymentStatus { get; set; } = string.Empty; // "Paid", "Unpaid"
    }
}
