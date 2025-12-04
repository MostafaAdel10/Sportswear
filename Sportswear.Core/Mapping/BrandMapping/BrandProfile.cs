using AutoMapper;

namespace Sportswear.Core.Mapping.BrandMapping
{
    public partial class BrandProfile : Profile
    {
        public BrandProfile()
        {
            GetBrandsListMapping();
            GetBrandByIdMapping();
            CreateBrandCommandMapping();
            EditBrandCommandMapping();
        }
    }
}
