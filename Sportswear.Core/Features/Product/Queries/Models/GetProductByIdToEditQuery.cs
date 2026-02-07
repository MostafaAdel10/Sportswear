using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductByIdToEditQuery : IRequest<Response<GetProductByIdToEditResponse>>
    {
        public GetProductByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
