namespace Sportswear.Core.Features.Product.Queries.Response_DTO_
{
    public class ProductVariantResponse
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public int StockQuantity { get; set; }

        public List<ProductVariantAttributeResponse> Attributes { get; set; } = new();
    }
}
