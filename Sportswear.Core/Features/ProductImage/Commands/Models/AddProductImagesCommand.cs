using MediatR;
using Microsoft.AspNetCore.Http;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductImage.Commands.Models
{
    public class AddProductImagesCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
