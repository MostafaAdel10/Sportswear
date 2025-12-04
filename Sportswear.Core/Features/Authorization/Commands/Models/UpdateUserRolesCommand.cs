using Sportswear.Core.Bases;
using Sportswear.DataAccess.Requests;
using MediatR;

namespace Sportswear.Core.Features.Authorization.Commands.Models
{
    public class UpdateUserRolesCommand : UpdateUserRolesRequest, IRequest<Response<string>>
    {
    }
}
