using Pos.Web.Shared.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Pos.Web.Infrastructure.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly byte[] _logoImage;

        public InvoiceDocument(InvoiceModel model, byte[] logoImage)
        {
            Model = model;
            _logoImage = logoImage;
        }

        public InvoiceModel Model { get; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.SegoeUI));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Left: Logo
                row.ConstantItem(100).Image(_logoImage);

                // Right: Invoice Label & Number
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"INVOICE #{Model.InvoiceNumber}")
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue Date: ").SemiBold();
                        text.Span($"{Model.IssueDate:d}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Status: ").SemiBold();
                        var color = Model.PaymentStatus == "Paid" ? Colors.Green.Medium : Colors.Red.Medium;
                        text.Span(Model.PaymentStatus).FontColor(color);
                    });
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                // Addresses
                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
                    row.ConstantItem(50); // Spacer
                    row.RelativeItem().Component(new AddressComponent("Bill To", Model.CustomerAddress));
                });

                column.Item().PaddingTop(25).Element(ComposeTable);

                // Totals Section (Right Aligned)
                column.Item().PaddingTop(25).AlignRight().Column(c =>
                {
                    c.Item().Text($"Subtotal: {Model.SubTotal:C}");
                    c.Item().Text($"Tax: {Model.TaxAmount:C}");
                    c.Item().Text($"Shipping: {Model.ShippingFee:C}");
                    c.Item().Text($"Discount: -{Model.DiscountAmount:C}").FontColor(Colors.Red.Medium);

                    c.Item().PaddingTop(10).Text($"Total: {Model.TotalAmount:C}")
                        .FontSize(14).Bold();
                });
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                // Define Columns
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25); // #
                    columns.RelativeColumn(3);  // Item Name
                    columns.RelativeColumn();   // Unit Price
                    columns.RelativeColumn();   // Quantity
                    columns.RelativeColumn();   // Total
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Item");
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                    header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                });

                // Rows
                foreach (var item in Model.Items)
                {
                    table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice:C}");
                    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Total:C}");

                    static IContainer CellStyle(IContainer container) =>
                        container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Thank you for your business!");
                //x.Item().PaddingTop(10).Text("Generated by PosSystem");
            });
        }
    }

    // Helper Component for Address
    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private AddressModel Address { get; }

        public AddressComponent(string title, AddressModel address)
        {
            Title = title;
            Address = address;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text(Title).SemiBold().FontColor(Colors.Grey.Medium);
                column.Item().Text(Address.CompanyName).Bold();
                column.Item().Text(Address.Street);
                column.Item().Text(Address.City);
                column.Item().Text(Address.Email);
            });
        }
    }
}
