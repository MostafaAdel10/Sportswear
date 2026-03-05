using Sportswear.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class ProductAttributeTemplate : AuditableEntity
    {
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required, MaxLength(100)]
        public string KeyEn { get; set; }

        [Required, MaxLength(100)]
        public string KeyAr { get; set; }

        public AttributeType Type { get; set; }

        public ICollection<ProductVariantAttribute> VariantAttributes { get; set; }
            = new List<ProductVariantAttribute>();
    }
}
