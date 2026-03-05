namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class VariantAttributeDto
    {
        public int TemplateId { get; set; }
        public string ValueEn { get; set; }
        public string ValueAr { get; set; }
        public string? ColorHex { get; set; }
    }
}
