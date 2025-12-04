using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Review.Queries.Models
{
    public class GetReviewsByProductIdQuery : IRequest<Response<List<ReviewDto>>>
    {
        public GetReviewsByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
        public int ProductId { get; set; }
    }
}
