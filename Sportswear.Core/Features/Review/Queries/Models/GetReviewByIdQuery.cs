using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Review.Queries.Models
{
    public class GetReviewByIdQuery : IRequest<Response<ReviewDto>>
    {
        public GetReviewByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
