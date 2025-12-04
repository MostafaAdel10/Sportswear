using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Order.Queries.Models
{
    public class GetOrderByIdQuery : IRequest<Response<OrderDto>>
    {
        public GetOrderByIdQuery(int orderId)
        {
            OrderId = orderId;
        }
        public int OrderId { get; set; }
    }
}
