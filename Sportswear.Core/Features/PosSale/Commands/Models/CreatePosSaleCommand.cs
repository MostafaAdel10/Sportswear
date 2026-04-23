using MediatR;
using Sportswear.Core.Bases;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.PosSale.Commands.Models
{
    public class CreatePosSaleCommand : IRequest<Response<int>>
    {
        public PosPaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public List<PosSaleItemDto> Items { get; set; }
    }
}
