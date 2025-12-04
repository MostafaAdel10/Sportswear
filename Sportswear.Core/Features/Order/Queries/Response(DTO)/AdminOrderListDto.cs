namespace Sportswear.Core.Features.Order.Queries.Response_DTO_
{
    public class AdminOrderListDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
