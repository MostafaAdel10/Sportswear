namespace Sportswear.Core.Features.Discount.Queries.Response_DTO_
{
    public class GetDiscountByIdResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; } // Mapped from DiscountType Enum
    }
}
