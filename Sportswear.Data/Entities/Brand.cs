using System.ComponentModel.DataAnnotations;

namespace Sportswear.DataAccess.Entities
{
    public class Brand : AuditableEntity
    {
        [Required, MaxLength(200)]
        public string NameEn { get; set; }

        [Required, MaxLength(200)]
        public string NameAr { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
