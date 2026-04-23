using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.PosSale.Commands.Models
{
    public class CancelPosSaleCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public CancelPosSaleCommand(int id) => Id = id;
    }
}
