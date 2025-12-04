using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductImage.Commands.Models
{
    public class DeleteProductImageCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
