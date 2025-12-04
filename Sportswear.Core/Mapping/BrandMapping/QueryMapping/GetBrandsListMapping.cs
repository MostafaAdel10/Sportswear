using Sportswear.Core.Features.Brand.Queries.Response_DTO_;
using Sportswear.DataAccess.Commons;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.BrandMapping
{
    public partial class BrandProfile
    {
        public void GetBrandsListMapping()
        {
            CreateMap<Brand, GetBrandsListResponse>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)));
        }
    }
}
