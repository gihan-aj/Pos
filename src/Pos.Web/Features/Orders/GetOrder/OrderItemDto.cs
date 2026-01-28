namespace Pos.Web.Features.Orders.GetOrder
{
    public sealed record OrderItemDto(
        Guid Id,
        Guid? ProductId,
        Guid VariantId,
        string ProductName,
        string? Variant,
        string Sku,
        int Quantity,
        int? MaxQuantity,
        decimal Price
    );
}
