using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductVariant.Queries.Response_DTO_;

namespace Sportswear.Core.Features.ProductVariant.Queries.Models
{
    public class GetProductVariantByIdToEditQuery : IRequest<Response<GetProductVariantByIdToEditResponse>>
    {
        public GetProductVariantByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
