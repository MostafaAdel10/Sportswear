using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class ProductVariantAttribute : AuditableEntity
    {
        [Required]
        [ForeignKey("ProductVariant")]
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        [Required]
        [ForeignKey("ProductAttributeTemplate")]
        public int ProductAttributeTemplateId { get; set; }
        public ProductAttributeTemplate ProductAttributeTemplate { get; set; }

        [Required, MaxLength(100)]
        public string ValueEn { get; set; }

        [Required, MaxLength(100)]
        public string ValueAr { get; set; }

        // للألوان بس
        [MaxLength(10)]
        public string? ColorHex { get; set; }
    }
}
