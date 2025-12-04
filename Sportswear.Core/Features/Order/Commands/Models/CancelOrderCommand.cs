using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Order.Commands.Models
{
    public class CancelOrderCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public CancelOrderCommand(int orderId)
        {
            OrderId = orderId;
        }
    }
}
