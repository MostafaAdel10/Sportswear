namespace Sportswear.Core.Features.Order.Queries.Response_DTO_
{
    public class OrderItemDto
    {
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public List<OrderItemAttributeDto> Attributes { get; set; } = new();
    }
}
