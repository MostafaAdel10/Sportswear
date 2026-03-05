using MediatR;
using Microsoft.AspNetCore.Http;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Category.Commands.Models
{
    public class CreateCategoryCommand : IRequest<Response<int>>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public IFormFile Image { get; set; }
    }
}
