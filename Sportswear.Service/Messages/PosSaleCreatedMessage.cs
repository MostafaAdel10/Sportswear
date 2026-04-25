namespace Sportswear.Service.Messages
{
    public class PosSaleCreatedMessage
    {
        public int SaleId { get; set; }
        public string SaleNumber { get; set; }
        public decimal FinalAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
