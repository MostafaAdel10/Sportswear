namespace Sportswear.Core.Features.Product.Queries.Response_DTO_
{
    public class GetProductByIdResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Season { get; set; }
        public string? Club { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        //ForeignKey
        public string BrandName { get; set; }
        public string CategoryName { get; set; }

        public List<string> Images { get; set; }

    }
}
