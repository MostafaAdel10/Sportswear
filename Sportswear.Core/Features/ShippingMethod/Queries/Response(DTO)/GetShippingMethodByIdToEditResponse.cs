namespace Sportswear.Core.Features.ShippingMethod.Queries.Response_DTO_
{
    public class GetShippingMethodByIdToEditResponse
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
    }
}
