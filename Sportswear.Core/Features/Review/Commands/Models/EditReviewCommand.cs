using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Review.Commands.Models
{
    public class EditReviewCommand : IRequest<Response<ReviewDto>>
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
