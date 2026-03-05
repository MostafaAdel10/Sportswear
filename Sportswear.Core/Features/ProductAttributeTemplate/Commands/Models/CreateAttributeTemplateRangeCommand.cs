using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Commands.Models
{
    public class CreateAttributeTemplateRangeCommand : IRequest<Response<string>>
    {
        public int CategoryId { get; set; }
        public List<AttributeTemplateDto> Templates { get; set; } = new();
    }
}
