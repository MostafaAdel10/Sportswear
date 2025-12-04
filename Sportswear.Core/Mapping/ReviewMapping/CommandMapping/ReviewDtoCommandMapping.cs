using AutoMapper;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ReviewMapping
{
    public partial class ReviewProfile : Profile
    {
        public void ReviewDtoCommandMapping()
        {
            CreateMap<Review, ReviewDto>();
        }
    }
}
