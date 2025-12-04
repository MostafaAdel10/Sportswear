using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.User.Commands.Models
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
