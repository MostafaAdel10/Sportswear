using Sportswear.Core.Bases;
using Sportswear.Core.Features.Authorization.Queries.Response_DTO_;
using MediatR;

namespace Sportswear.Core.Features.Authorization.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<GetRoleByIdResponse>>
    {
        public int Id { get; set; }
    }
}
