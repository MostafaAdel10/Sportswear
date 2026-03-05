using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class CreateProductVariantRangeCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public List<CreateProductVariantDto> Variants { get; set; } = new();
    }
}
