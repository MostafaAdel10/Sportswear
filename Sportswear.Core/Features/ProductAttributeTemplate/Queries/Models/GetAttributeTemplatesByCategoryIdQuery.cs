using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductAttributeTemplate.Queries.Response_DTO_;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Queries.Models
{
    public class GetAttributeTemplatesByCategoryIdQuery : IRequest<Response<List<AttributeTemplateResponse>>>
    {
        public int CategoryId { get; set; }
        public GetAttributeTemplatesByCategoryIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
