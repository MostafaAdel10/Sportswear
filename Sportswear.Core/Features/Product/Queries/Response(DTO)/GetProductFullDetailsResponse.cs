namespace Sportswear.Core.Features.Product.Queries.Response_DTO_
{
    public class GetProductFullDetailsResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string? Season { get; set; }
        public string? ClubEn { get; set; }
        public string? ClubAr { get; set; }

        // Brand
        public int BrandId { get; set; }
        public string BrandNameEn { get; set; }
        public string BrandNameAr { get; set; }

        // Category
        public int CategoryId { get; set; }
        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }

        // Pricing
        public decimal BasePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPriceAfterDiscount { get; set; }
        public decimal MaxPriceAfterDiscount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public bool HasVariants { get; set; }

        // Images
        public List<string> Images { get; set; } = new();

        // Variants
        public List<FullProductVariantDto> Variants { get; set; } = new();

        // Discounts
        public List<ProductDiscountDto> Discounts { get; set; } = new();

        // Reviews
        public List<ProductReviewDto> Reviews { get; set; } = new();
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }

        // Attributes
        public string? AttributeKeyEn { get; set; }
        public string? AttributeKeyAr { get; set; }
    }

    public class FullProductVariantDto
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public int StockQuantity { get; set; }
        public bool InStock => StockQuantity > 0;
        public string? AttributeValueEn { get; set; }
        public string? AttributeValueAr { get; set; }
        public string? Unit { get; set; }
        public string? ColorLabel { get; set; }
        public string? ColorHex { get; set; }
    }

    public class ProductDiscountDto
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductReviewDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
