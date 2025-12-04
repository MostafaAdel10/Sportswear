namespace Sportswear.Core.Features.Order.Queries.Response_DTO_
{
    public class ShipmentDto
    {
        public string FullName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string StreetAddress { get; set; }
        public int? BuildingNumber { get; set; }
        public int? FloorNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string? Notes { get; set; }
    }
}
