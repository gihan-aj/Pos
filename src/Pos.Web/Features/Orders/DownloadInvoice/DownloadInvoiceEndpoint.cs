using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Dtos;

namespace Pos.Web.Features.Orders.DownloadInvoice
{
    public static class DownloadInvoiceEndpoint
    {
        public static void MapDownloadInvoice(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:guid}/invoice", async (Guid id, AppDbContext db, IInvoiceGenerator generator) =>
            {
                // 1. Fetch Data
                var order = await db.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order is null) return Results.NotFound();

                // 2. Map Entity -> DTO
                var model = new InvoiceModel
                {
                    InvoiceNumber = order.OrderNumber,
                    IssueDate = order.OrderDate,
                    DueDate = order.OrderDate.AddDays(7), // Example logic
                    PaymentStatus = order.OrderPaymentStatus.ToString(),

                    SellerAddress = new AddressModel
                    {
                        CompanyName = "My Clothing Store",
                        Street = "123 Fashion Ave",
                        City = "Colombo",
                        Email = "support@store.com"
                    },

                    CustomerAddress = new AddressModel
                    {
                        CompanyName = order.Customer?.Name ?? "Guest",
                        Street = order.DeliveryAddress,
                        City = $"{order.DeliveryCity}, {order.DeliveryCountry}",
                        Email = order.Customer?.Email ?? ""
                    },

                    Items = order.OrderItems.Select(i => new InvoiceItemModel
                    {
                        Name = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Total = i.SubTotal
                    }).ToList(),

                    SubTotal = order.SubTotal,
                    TaxAmount = order.TaxAmount,
                    ShippingFee = order.ShippingFee,
                    DiscountAmount = order.DiscountAmount,
                    TotalAmount = order.TotalAmount
                };

                // 3. Generate
                var pdfBytes = generator.Generate(model);

                // 4. Return File
                return Results.File(pdfBytes, "application/pdf", $"Invoice-{order.OrderNumber}.pdf");
            });
        }
    }
}
