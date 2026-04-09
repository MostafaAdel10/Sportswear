namespace Sportswear.Core.Features.Order.Queries.Response_DTO_
{
    public class OrderItemDto
    {
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string? AttributeKey { get; set; }
        public string? AttributeValue { get; set; }
        public string? Unit { get; set; }
        public string? ColorLabel { get; set; }
        public string? ColorHex { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
