using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Order.Queries.Models
{
    public class GetAllOrdersQuery : IRequest<Response<List<AdminOrderListDto>>>
    {
    }
}
