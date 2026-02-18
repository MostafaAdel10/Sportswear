using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class CreateProductVariantRangeCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public List<CreateProductVariantItemDto> Variants { get; set; }
    }

    public class CreateProductVariantItemDto
    {
        public string Size { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
