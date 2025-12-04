using Sportswear.DataAccess.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class Cart : BaseEntity
    {
        [Required]
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
