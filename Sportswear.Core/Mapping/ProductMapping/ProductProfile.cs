using AutoMapper;

namespace Sportswear.Core.Mapping.ProductMapping
{
    public partial class ProductProfile : Profile
    {
        public ProductProfile()
        {
            GetProductsListMapping();
            GetProductByIdMapping();
            CreateProductCommandMapping();
            EditProductCommandMapping();
            GetProductByIdToEditMapping();
        }
    }
}
