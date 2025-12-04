using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.User.Queries.Response_DTO_;

namespace Sportswear.Core.Features.User.Queries.Models
{
    public class GetUserByIdQuery : IRequest<Response<GetUserByIdResponse>>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
