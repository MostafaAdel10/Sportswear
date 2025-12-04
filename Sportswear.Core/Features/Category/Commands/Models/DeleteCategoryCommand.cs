using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Category.Commands.Models
{
    public class DeleteCategoryCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
