using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.CartItem.Commands.Models
{
    public class AddCartItemCommand : IRequest<Response<string>>
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
