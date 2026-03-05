using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class Product : AuditableEntity
    {
        [Required, MaxLength(100)]
        public string Code { get; set; }

        [Required, MaxLength(200)]
        public string NameEn { get; set; }

        [Required, MaxLength(200)]
        public string NameAr { get; set; }

        [Required]
        public string DescriptionEn { get; set; }

        [Required]
        public string DescriptionAr { get; set; }

        [MaxLength(100)]
        public string? Season { get; set; }

        [MaxLength(100)]
        public string? ClubEn { get; set; }

        [MaxLength(100)]
        public string? ClubAr { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxPrice { get; set; }

        public bool HasVariants { get; set; } = false;

        [Required]
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Product_Discount> Product_Discounts { get; set; } = new List<Product_Discount>();
    }
}
