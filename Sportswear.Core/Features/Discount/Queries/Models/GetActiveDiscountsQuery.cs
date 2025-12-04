using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Discount.Queries.Models
{
    public class GetActiveDiscountsQuery : IRequest<Response<List<GetActiveDiscountsResponse>>>
    {
    }
}
