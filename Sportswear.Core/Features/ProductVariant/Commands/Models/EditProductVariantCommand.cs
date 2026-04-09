using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class EditProductVariantCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string? AttributeValueEn { get; set; }
        public string? AttributeValueAr { get; set; }
        public string? Unit { get; set; }
        public string? ColorLabel { get; set; }
        public string? ColorHex { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
