using Sportswear.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sportswear.DataAccess.Entities
{
    public class Discount : AuditableEntity
    {
        [Required, MaxLength(100)]
        public string Code { get; set; }

        [Required, MaxLength(200)]
        public string NameEn { get; set; }

        [Required, MaxLength(200)]
        public string NameAr { get; set; }

        [Range(0, 100)]
        public decimal Percentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DiscountType Type { get; set; }

        public ICollection<Product_Discount> Product_Discounts { get; set; } = new List<Product_Discount>();
    }
}
