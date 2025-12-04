using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Brand.Commands.Models
{
    public class CreateBrandCommand : IRequest<Response<string>>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
