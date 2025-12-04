using MediatR;
using Sportswear.Core.Bases;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.Order.Commands.Models
{
    public class ChangePaymentStatusCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
