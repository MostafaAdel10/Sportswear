using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Discount.Queries.Models
{
    public class GetActiveDiscountByIdToEditQuery : IRequest<Response<GetDiscountByIdToEditResponse>>
    {
        public GetActiveDiscountByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
