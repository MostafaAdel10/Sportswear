using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class DeleteProductVariantCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteProductVariantCommand(int id)
        {
            Id = id;
        }
    }
}
