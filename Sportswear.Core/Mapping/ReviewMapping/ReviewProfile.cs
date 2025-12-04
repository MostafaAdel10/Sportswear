using AutoMapper;

namespace Sportswear.Core.Mapping.ReviewMapping
{
    public partial class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            AddReviewCommandMapping();
            EditReviewCommandMapping();
            GetReviewByIdMapping();
            GetReviewsByProductIdMapping();
            ReviewDtoCommandMapping();
        }
    }
}
