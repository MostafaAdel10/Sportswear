using Sportswear.Core.Features.Category.Queries.Response_DTO_;
using Sportswear.DataAccess.Commons;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.CategoryMapping
{
    public partial class CategoryProfile
    {
        public void GetCategoryByIdMapping()
        {
            CreateMap<Category, GetCategoryByIdResponse>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)));
        }
    }
}
