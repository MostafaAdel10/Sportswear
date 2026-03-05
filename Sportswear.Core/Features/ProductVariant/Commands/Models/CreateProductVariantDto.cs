namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class CreateProductVariantDto
    {
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<VariantAttributeDto> Attributes { get; set; } = new();
    }
}
