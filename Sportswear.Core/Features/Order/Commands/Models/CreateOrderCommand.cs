using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Order.Commands.Models
{
    public class CreateOrderCommand : IRequest<Response<int>>
    {
        public int ShippingMethodId { get; set; }
        public ShipmentDto Shipment { get; set; }
    }
}
