using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Category.Queries.Models
{
    public class GetCategoriesListQuery : IRequest<Response<List<GetCategoriesListResponse>>>
    {
    }
}
