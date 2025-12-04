using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Category.Commands.Models
{
    public class CreateCategoryCommand : IRequest<Response<string>>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
