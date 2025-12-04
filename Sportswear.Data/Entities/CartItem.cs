using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class CartItem : BaseEntity
    {
        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [Required]
        [ForeignKey("ProductVariant")]
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int Quantity { get; set; }
    }
}
