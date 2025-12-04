using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Discount.Commands.Models
{
    public class DeleteDiscountCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteDiscountCommand(int id)
        {
            Id = id;
        }
    }
}
