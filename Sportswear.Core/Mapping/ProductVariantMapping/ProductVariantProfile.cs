using AutoMapper;

namespace Sportswear.Core.Mapping.ProductVariantMapping
{
    public partial class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateProductVariantCommandMapping();
            EditProductVariantCommandMapping();
        }
    }
}
