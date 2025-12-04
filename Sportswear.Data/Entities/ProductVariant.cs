using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class ProductVariant : AuditableEntity
    {
        [Required, MaxLength(50)]
        public string Size { get; set; }

        [Required, MaxLength(50)]
        public string Color { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // لو السعر مختلف

        public int StockQuantity { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
