namespace Sportswear.Core.Features.PosSale.Queries.Response_DTO_
{
    public class GetPosSalesListResponse
    {
        public int Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public int ItemsCount { get; set; }
    }
}
