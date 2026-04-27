namespace Sportswear.Core.Features.PosSale.Queries.Response_DTO_
{
    public class PosSaleItemResponse
    {
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string? AttributeValue { get; set; }
        public string? Color { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
