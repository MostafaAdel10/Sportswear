using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductByIdQuery : IRequest<Response<GetProductByIdResponse>>
    {
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
