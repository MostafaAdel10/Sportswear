using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Brand.Commands.Models
{
    public class DeleteBrandCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteBrandCommand(int id)
        {
            Id = id;
        }
    }
}
