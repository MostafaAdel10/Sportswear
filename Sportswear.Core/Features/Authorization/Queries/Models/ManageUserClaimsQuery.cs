using Sportswear.Core.Bases;
using Sportswear.DataAccess.Results;
using MediatR;

namespace Sportswear.Core.Features.Authorization.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManageUserClaimsResult>>
    {
        public int UserId { get; set; }
    }
}
