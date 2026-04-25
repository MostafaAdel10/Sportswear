namespace Sportswear.Service.Messages
{
    public class OrderCreatedMessage
    {
        public int OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
