using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Product.Commands.Models
{
    public class DeleteProductCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
