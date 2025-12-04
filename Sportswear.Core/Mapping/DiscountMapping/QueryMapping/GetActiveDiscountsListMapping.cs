using AutoMapper;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;
using Sportswear.DataAccess.Commons;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.DiscountMapping
{
    public partial class DiscountProfile : Profile
    {
        public void GetActiveDiscountsListMapping()
        {
            CreateMap<Discount, GetActiveDiscountsResponse>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
