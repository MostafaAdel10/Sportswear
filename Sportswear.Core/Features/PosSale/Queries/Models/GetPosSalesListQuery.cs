using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.PosSale.Queries.Response_DTO_;

namespace Sportswear.Core.Features.PosSale.Queries.Models
{
    public class GetPosSalesListQuery : IRequest<Response<List<GetPosSalesListResponse>>>
    {
    }
}
