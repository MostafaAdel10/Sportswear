namespace Sportswear.Core.Features.Discount.Queries.Response_DTO_
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }        // "Active", "Expired", "Upcoming"
        public int DaysRemaining { get; set; }    // لو Active كام يوم فاضل
        public int ProductsCount { get; set; }    // كام منتج بيستخدمه
        public string Type { get; set; }
    }
}
