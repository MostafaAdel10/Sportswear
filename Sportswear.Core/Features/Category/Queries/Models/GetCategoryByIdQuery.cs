using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Category.Queries.Models
{
    public class GetCategoryByIdQuery : IRequest<Response<GetCategoryByIdResponse>>
    {
        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
