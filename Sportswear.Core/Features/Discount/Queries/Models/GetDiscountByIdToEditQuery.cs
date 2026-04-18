using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Discount.Queries.Models
{
    public class GetDiscountByIdToEditQuery : IRequest<Response<GetDiscountByIdToEditResponse>>
    {
        public GetDiscountByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
