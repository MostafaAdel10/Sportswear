using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Category.Commands.Models
{
    public class EditCategoryCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
