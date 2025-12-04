using MediatR;
using Microsoft.AspNetCore.Http;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductImage.Commands.Models
{
    public class AddProductImageCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; }
    }
}
