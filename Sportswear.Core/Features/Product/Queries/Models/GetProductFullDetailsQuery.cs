using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductFullDetailsQuery : IRequest<Response<GetProductFullDetailsResponse>>
    {
        public int Id { get; set; }
        public GetProductFullDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
