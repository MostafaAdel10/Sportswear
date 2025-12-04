using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.CartItem.Commands.Models
{
    public class EditCartItemCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
