using System.ComponentModel.DataAnnotations;

namespace Sportswear.DataAccess.Entities
{
    public class Category : AuditableEntity
    {
        [Required, MaxLength(200)]
        public string NameEn { get; set; }

        [Required, MaxLength(200)]
        public string NameAr { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
