namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class CreateProductVariantDto
    {
        public string? AttributeValueEn { get; set; }
        public string? AttributeValueAr { get; set; }
        public string? Unit { get; set; }
        public string? ColorLabel { get; set; }
        public string? ColorHex { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
