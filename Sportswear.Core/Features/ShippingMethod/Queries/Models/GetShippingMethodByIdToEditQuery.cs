using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ShippingMethod.Queries.Response_DTO_;

namespace Sportswear.Core.Features.ShippingMethod.Queries.Models
{
    public class GetShippingMethodByIdToEditQuery : IRequest<Response<GetShippingMethodByIdToEditResponse>>
    {
        public GetShippingMethodByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
