namespace Sportswear.Core.Features.Product.Queries.Response_DTO_
{
    public class ProductVariantResponse
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public int StockQuantity { get; set; }
        public string? AttributeValueEn { get; set; }
        public string? AttributeValueAr { get; set; }
        public string? Unit { get; set; }
        public string? ColorLabel { get; set; }
        public string? ColorHex { get; set; }
    }
}
