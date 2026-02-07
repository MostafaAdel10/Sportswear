using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Category.Queries.Models
{
    public class GetCategoryByIdToEditQuery : IRequest<Response<GetCategoryByIdToEditResponse>>
    {
        public GetCategoryByIdToEditQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
