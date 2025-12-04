using Sportswear.DataAccess.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class Review : AuditableEntity
    {
        [Required]
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required, MaxLength(1000)]
        public string Comment { get; set; }
    }
}
