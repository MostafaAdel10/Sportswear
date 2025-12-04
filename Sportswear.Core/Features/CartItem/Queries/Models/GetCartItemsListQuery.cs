using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.CartItem.Queries.Response_DTO_;

namespace Sportswear.Core.Features.CartItem.Queries.Models
{
    public class GetCartItemsListQuery : IRequest<Response<List<CartItemDto>>>
    {
    }
}
