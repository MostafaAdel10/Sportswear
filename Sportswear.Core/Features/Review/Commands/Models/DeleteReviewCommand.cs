using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Review.Commands.Models
{
    public class DeleteReviewCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteReviewCommand(int id)
        {
            Id = id;
        }
    }
}
