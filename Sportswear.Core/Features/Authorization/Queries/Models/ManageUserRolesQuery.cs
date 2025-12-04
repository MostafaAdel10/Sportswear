using Sportswear.Core.Bases;
using Sportswear.DataAccess.Results;
using MediatR;

namespace Sportswear.Core.Features.Authorization.Queries.Models
{
    public class ManageUserRolesQuery : IRequest<Response<ManageUserRolesResult>>
    {
        public int UserId { get; set; }
    }
}
