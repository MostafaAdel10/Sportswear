using AutoMapper;
using Sportswear.Core.Features.Review.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ReviewMapping
{
    public partial class ReviewProfile : Profile
    {
        public void AddReviewCommandMapping()
        {
            CreateMap<AddReviewCommand, Review>();
        }
    }
}
