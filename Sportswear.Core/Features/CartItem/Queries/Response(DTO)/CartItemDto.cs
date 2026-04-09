namespace Sportswear.Core.Features.CartItem.Queries.Response_DTO_
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string? ProductImageUrl { get; set; }
        public string SKU { get; set; }
        public string? AttributeKey { get; set; }
        public string? AttributeValue { get; set; }
        public string? Unit { get; set; }
        public string? ColorLabel { get; set; }
        public string? ColorHex { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
