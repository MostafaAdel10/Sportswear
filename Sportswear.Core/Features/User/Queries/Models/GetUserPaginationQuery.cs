using MediatR;
using Sportswear.Core.Features.User.Queries.Response_DTO_;
using Sportswear.Core.Wrappers;

namespace Sportswear.Core.Features.User.Queries.Models
{
    public class GetUserPaginationQuery : IRequest<PaginatedResult<GetUserPaginationReponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
