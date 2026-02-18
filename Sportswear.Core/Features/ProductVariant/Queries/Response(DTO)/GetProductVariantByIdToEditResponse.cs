namespace Sportswear.Core.Features.ProductVariant.Queries.Response_DTO_
{
    public class GetProductVariantByIdToEditResponse
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
