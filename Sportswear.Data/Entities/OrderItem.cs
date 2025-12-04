using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class OrderItem : BaseEntity
    {
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        [ForeignKey("ProductVariant")]
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}
