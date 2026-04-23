using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class ProductVariant : AuditableEntity
    {
        [Required, MaxLength(100)]
        public string SKU { get; set; }

        [MaxLength(200)]
        public string? AttributeValueEn { get; set; }  // "XL", "10kg"

        [MaxLength(200)]
        public string? AttributeValueAr { get; set; }  // "كبير", "١٠ كيلو"

        [MaxLength(50)]
        public string? Unit { get; set; }               // "kg", "cm" or null

        [MaxLength(100)]
        public string? ColorLabel { get; set; }         // "Red", "Blue" or null

        [MaxLength(10)]
        public string? ColorHex { get; set; }           // "#FF0000" or null

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<PosSaleItem> PosSaleItems { get; set; } = new List<PosSaleItem>();
    }
}
