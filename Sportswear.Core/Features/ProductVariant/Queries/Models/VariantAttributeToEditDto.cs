namespace Sportswear.Core.Features.ProductVariant.Queries.Models
{
    public class VariantAttributeToEditDto
    {
        public int TemplateId { get; set; }
        public string KeyEn { get; set; }
        public string KeyAr { get; set; }
        public string Type { get; set; }
        public string ValueEn { get; set; }
        public string ValueAr { get; set; }
        public string? ColorHex { get; set; }
    }
}
