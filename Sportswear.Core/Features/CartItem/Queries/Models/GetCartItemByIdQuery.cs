using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.CartItem.Queries.Response_DTO_;

namespace Sportswear.Core.Features.CartItem.Queries.Models
{
    public class GetCartItemByIdQuery : IRequest<Response<CartItemDto>>
    {
        public GetCartItemByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
