using Sportswear.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sportswear.DataAccess.Entities
{
    public class Payment : BaseEntity
    {
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public PaymentMethod Method { get; set; } // Card, CashOnDelivery, PayPal
        public PaymentStatus Status { get; set; } // Pending, Completed, Failed

        public DateTime? PaidAt { get; set; }
    }
}
