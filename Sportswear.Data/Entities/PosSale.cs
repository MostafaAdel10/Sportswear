using Sportswear.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class PosSale : AuditableEntity
    {
        [Required, MaxLength(50)]
        public string SaleNumber { get; set; } // POS-00001

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }

        public PosPaymentMethod PaymentMethod { get; set; }

        public PosSaleStatus Status { get; set; } = PosSaleStatus.Completed;

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation
        public ICollection<PosSaleItem> Items { get; set; } = new List<PosSaleItem>();
    }
}
