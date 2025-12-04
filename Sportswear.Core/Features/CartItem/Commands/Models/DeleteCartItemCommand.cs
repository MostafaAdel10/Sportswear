using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.CartItem.Commands.Models
{
    public class DeleteCartItemCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCartItemCommand(int id)
        {
            Id = id;
        }
    }
}
