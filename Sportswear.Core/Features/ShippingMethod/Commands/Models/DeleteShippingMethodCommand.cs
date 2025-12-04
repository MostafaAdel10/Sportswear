using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ShippingMethod.Commands.Models
{
    public class DeleteShippingMethodCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteShippingMethodCommand(int id)
        {
            Id = id;
        }
    }
}
