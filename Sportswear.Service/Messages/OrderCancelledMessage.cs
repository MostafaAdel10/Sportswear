namespace Sportswear.Service.Messages
{
    public class OrderCancelledMessage
    {
        public int OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}
