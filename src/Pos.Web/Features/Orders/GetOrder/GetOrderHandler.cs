using Microsoft.EntityFrameworkCore;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Features.Orders.GetOrder
{
    public class GetOrderHandler : IQueryHandler<GetOrderQuery, GetOrderResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetOrderResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductVariant)
                .Include(o => o.Payments)
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order is null)
                return Result.Failure<GetOrderResponse>(Error.NotFound("Order.NotFound", "Order was not found."));

            var response = new GetOrderResponse(
                order.Id,
                order.OrderNumber,
                order.CustomerId,
                order.Customer != null 
                ? new CustomerDto(
                    order.Customer.Id, 
                    order.Customer.Name, 
                    order.Customer.PhoneNumber, 
                    order.Customer.Email, 
                    order.Customer.Address, 
                    order.Customer.City,
                    order.Customer.Country,
                    order.Customer.PostalCode,
                    order.Customer.Region,
                    order.Customer.Notes) 
                : null,
                order.OrderDate,
                order.Status,
                order.OrderPaymentStatus,
                order.SubTotal,
                order.DiscountAmount,
                order.TaxAmount,
                order.ShippingFee,
                order.TotalAmount,
                order.AmountPaid,
                order.AmountDue,
                order.IsCashOnDelivery,
                order.DeliveryAddress,
                order.DeliveryCity,
                order.DeliveryRegion,
                order.DeliveryCountry,
                order.DeliveryPostalCode,
                order.CourierId,
                order.Courier?.Name,
                order.TrackingNumber,
                order.Notes,
                order.OrderItems.Select(i => new OrderItemDto(
                    i.Id,
                    i.ProductVariant?.ProductId,
                    i.ProductVariantId,
                    i.ProductName,
                    i.VariantDetails,
                    i.Sku,
                    i.Quantity,
                    i.ProductVariant?.StockQuantity,
                    i.UnitPrice
                )).ToList(),
                order.Payments
                .Where(p => p.Status == Entities.PaymentStatus.Completed)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new OrderPaymentDto(
                    p.Id,
                    p.PaymentDate,
                    p.Method,
                    p.Amount,
                    p.TransactionId,
                    p.Status
                )).ToList()
            );

            return response;
        }
    }
}
