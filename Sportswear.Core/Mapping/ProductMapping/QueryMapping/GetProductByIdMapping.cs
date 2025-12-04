using Sportswear.Core.Features.Product.Queries.Response_DTO_;
using Sportswear.DataAccess.Commons;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ProductMapping
{
    public partial class ProductProfile
    {
        public void GetProductByIdMapping()
        {
            CreateMap<Product, GetProductByIdResponse>()
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.Localize(src.Brand.NameAr, src.Brand.NameEn)))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.Localize(src.Category.NameAr, src.Category.NameEn)))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Description, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Club, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
        }
    }
}
