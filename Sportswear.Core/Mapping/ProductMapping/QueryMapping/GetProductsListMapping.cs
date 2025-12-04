using Sportswear.Core.Features.Product.Queries.Response_DTO_;
using Sportswear.DataAccess.Commons;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ProductMapping
{
    public partial class ProductProfile
    {
        public void GetProductsListMapping()
        {
            //// The first way
            // الطريقه القديمه اني اخلي entities تعمل inhirt من GeneralLocalizableEntity class
            //CreateMap<Product, GetProductsListResponse>()
            //     .ForMember(dest => dest.BrandName, obtion => obtion.MapFrom(src => src.Brand.Localize(src.Brand.NameAr, src.Brand.NameEn)))
            //    .ForMember(dest => dest.CategoryName, obtion => obtion.MapFrom(src => src.Category.Localize(src.Category.NameAr, src.Brand.NameEn)));


            //// The second way
            //بدل الطريقه القديمه اني اخلي entities تعمل inhirt من GeneralLocalizableEntity class
            // خليت GeneralLocalizableEntity class static وعملت ميثود static جواها
            // فبالتالي اقدر استخدمها زي ما عملت تحت كده وده افضل

            //CreateMap<Product, GetProductsListResponse>()
            //    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => 
            //            GeneralLocalizableEntity.Localize(src.Brand.NameAr, src.Brand.NameEn)))
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => 
            //            GeneralLocalizableEntity.Localize(src.Category.NameAr, src.Category.NameEn)));


            //// The third way (Best way)
            // استخدمت extension method عشان ابسط الكود اكتر
            // فبالتالي اقدر استخدمها زي ما عملت تحت كده وده افضل
            // فبالتالي مش محتاج اغير اي حاجه في ال entities
            // ومش محتاج اخليها تعمل inhirt من GeneralLocalizableEntity class

            CreateMap<Product, GetProductsListResponse>()
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.Localize(src.Brand.NameAr, src.Brand.NameEn)))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.Localize(src.Category.NameAr, src.Category.NameEn)))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Description, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Club, obtion => obtion.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
        }
    }
}
