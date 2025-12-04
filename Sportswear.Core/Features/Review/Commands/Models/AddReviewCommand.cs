using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Review.Commands.Models
{
    public class AddReviewCommand : IRequest<Response<ReviewDto>>
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
