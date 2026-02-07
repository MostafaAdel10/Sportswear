using AutoMapper;

namespace Sportswear.Core.Mapping.CategoryMapping
{
    public partial class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            GetCategoriesListMapping();
            GetCategoryByIdMapping();
            GetCategoryByIdToEditMapping();
        }
    }
}
