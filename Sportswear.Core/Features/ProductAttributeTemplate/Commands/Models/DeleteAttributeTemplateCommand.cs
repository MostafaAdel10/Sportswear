using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Commands.Models
{
    public class DeleteAttributeTemplateCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteAttributeTemplateCommand(int id)
        {
            Id = id;
        }
    }
}
