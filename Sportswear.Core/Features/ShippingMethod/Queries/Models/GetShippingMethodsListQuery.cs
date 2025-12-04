using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ShippingMethod.Queries.Response_DTO_;

namespace Sportswear.Core.Features.ShippingMethod.Queries.Models
{
    public class GetShippingMethodsListQuery : IRequest<Response<List<GetShippingMethodsListResponse>>>
    {
    }
}
