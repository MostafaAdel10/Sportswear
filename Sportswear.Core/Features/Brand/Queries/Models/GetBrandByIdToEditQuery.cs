using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Brand.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Brand.Queries.Models
{
    public class GetBrandByIdToEditQuery : IRequest<Response<GetBrandByIdToEditResponse>>
    {
        public GetBrandByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
