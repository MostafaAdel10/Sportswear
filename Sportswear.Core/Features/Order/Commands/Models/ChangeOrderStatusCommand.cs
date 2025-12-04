using MediatR;
using Sportswear.Core.Bases;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.Order.Commands.Models
{
    public class ChangeOrderStatusCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
