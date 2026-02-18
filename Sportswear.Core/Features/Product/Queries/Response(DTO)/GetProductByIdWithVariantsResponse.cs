namespace Sportswear.Core.Features.Product.Queries.Response_DTO_
{
    public class GetProductByIdWithVariantsResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Season { get; set; }
        public string? Club { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        //ForeignKey
        public string BrandName { get; set; }
        public string CategoryName { get; set; }

        public List<string> Images { get; set; }

        public List<ProductVariantResponse> Variants { get; set; } = new();
    }

    public class ProductVariantResponse
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterDiscount { get; set; }
        public int StockQuantity { get; set; }
    }
}
