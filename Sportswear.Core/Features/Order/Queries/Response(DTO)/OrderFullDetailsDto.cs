namespace Sportswear.Core.Features.Order.Queries.Response_DTO_
{
    public class OrderFullDetailsDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // User
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }

        // Payment
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }

        // Shipment
        public ShipmentDto? ShipmentInfo { get; set; }

        // Items
        public List<OrderItemDto> Items { get; set; } = new();
        public int TotalQuantity { get; set; }
    }
}
