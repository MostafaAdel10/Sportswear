using Sportswear.Core.Bases;
using MediatR;

namespace Sportswear.Core.Features.Authorization.Commands.Models
{
    public class AddRoleCommand : IRequest<Response<string>>
    {
        public string RoleName { get; set; }
    }
}
