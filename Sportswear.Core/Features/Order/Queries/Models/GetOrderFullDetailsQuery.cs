using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Order.Queries.Models
{
    public class GetOrderFullDetailsQuery : IRequest<Response<OrderFullDetailsDto>>
    {
        public int OrderId { get; set; }
        public GetOrderFullDetailsQuery(int orderId)
        {
            OrderId = orderId;
        }
    }
}
