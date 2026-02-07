using MediatR;
using Microsoft.AspNetCore.Http;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Brand.Commands.Models
{
    public class CreateBrandCommand : IRequest<Response<string>>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public IFormFile Image { get; set; }
    }
}
