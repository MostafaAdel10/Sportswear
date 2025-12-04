using MediatR;
using Microsoft.AspNetCore.Http;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductImage.Commands.Models
{
    public class EditProductImageCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public string OldImageUrl { get; set; } = string.Empty;
        public IFormFile NewImage { get; set; }
    }
}
