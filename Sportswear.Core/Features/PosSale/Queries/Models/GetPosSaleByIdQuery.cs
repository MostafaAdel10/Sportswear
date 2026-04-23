using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.PosSale.Queries.Response_DTO_;

namespace Sportswear.Core.Features.PosSale.Queries.Models
{
    public class GetPosSaleByIdQuery : IRequest<Response<GetPosSaleByIdResponse>>
    {
        public int Id { get; set; }
        public GetPosSaleByIdQuery(int id) => Id = id;
    }
}
