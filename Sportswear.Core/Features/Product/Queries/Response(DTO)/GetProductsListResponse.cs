namespace Sportswear.Core.Features.Product.Queries.Response_DTO_
{
    public class GetProductsListResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Season { get; set; }
        public string? Club { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool HasVariants { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal MinPriceAfterDiscount { get; set; }
        public decimal MaxPriceAfterDiscount { get; set; }
        public string? AttributeKey { get; set; }
        public List<string> Images { get; set; } = new();
    }
}
