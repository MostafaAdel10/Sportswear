using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities
{
    public class ShippingMethod : AuditableEntity
    {
        [Required, MaxLength(100)]
        public string NameEn { get; set; } // مثال: Standard, Express, Pickup

        [Required, MaxLength(100)]
        public string NameAr { get; set; } // مثال: قياسي، سريع، العميل هو اللي ييجي ياخد الطلب بنفسه

        [MaxLength(400)]
        public string DescriptionEn { get; set; }

        [MaxLength(400)]
        public string DescriptionAr { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // تكلفة الشحن

        public int EstimatedDeliveryDays { get; set; } // عدد الأيام المتوقع

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
