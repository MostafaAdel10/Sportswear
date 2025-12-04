using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Order.Queries.Models
{
    public class GetOrdersByUserIdQuery : IRequest<Response<List<AdminOrderListDto>>>
    {
        public int UserId { get; set; }
        public GetOrdersByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
