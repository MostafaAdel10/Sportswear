using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductByIdWithVariantsQuery : IRequest<Response<GetProductByIdWithVariantsResponse>>
    {
        public GetProductByIdWithVariantsQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
