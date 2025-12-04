using Sportswear.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sportswear.DataAccess.Entities
{
    public class Shipment : BaseEntity
    {
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(100)]
        public string Region { get; set; }

        [Required, MaxLength(300)]
        public string StreetAddress { get; set; }

        public int? BuildingNumber { get; set; }
        public int? FloorNumber { get; set; }
        public int? ApartmentNumber { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        public string? Notes { get; set; }

        [Required]
        public int ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }

        [MaxLength(200)]
        public string? TrackingNumber { get; set; } // Optional

        public ShippingStatus Status { get; set; } // Processing, Shipped, Delivered

    }
}
