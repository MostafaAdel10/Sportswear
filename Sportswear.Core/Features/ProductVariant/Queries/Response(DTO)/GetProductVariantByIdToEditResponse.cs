using Sportswear.Core.Features.ProductVariant.Queries.Models;

namespace Sportswear.Core.Features.ProductVariant.Queries.Response_DTO_
{
    public class GetProductVariantByIdToEditResponse
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<VariantAttributeToEditDto> Attributes { get; set; } = new();
    }
}
