using Sportswear.Core.Features.Category.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.CategoryMapping
{
    public partial class CategoryProfile
    {
        public void GetCategoryByIdToEditMapping()
        {
            CreateMap<Category, GetCategoryByIdToEditResponse>();
        }
    }
}
