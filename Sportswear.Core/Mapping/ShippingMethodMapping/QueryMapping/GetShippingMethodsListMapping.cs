using AutoMapper;
using Sportswear.Core.Features.ShippingMethod.Queries.Response_DTO_;
using Sportswear.DataAccess.Commons;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ShippingMethodMapping
{
    public partial class ShippingMethodProfile : Profile
    {
        public void GetShippingMethodsListMapping()
        {
            CreateMap<ShippingMethod, GetShippingMethodsListResponse>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Description, obtion => obtion.MapFrom(src => src.Localize(src.DescriptionAr, src.DescriptionEn)));
        }
    }
}
