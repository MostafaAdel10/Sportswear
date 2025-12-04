using Sportswear.Core.Bases;
using Sportswear.DataAccess.Requests;
using MediatR;

namespace Sportswear.Core.Features.Authorization.Commands.Models
{
    public class EditRoleCommand : EditRoleRequest, IRequest<Response<string>>
    {
    }
}
