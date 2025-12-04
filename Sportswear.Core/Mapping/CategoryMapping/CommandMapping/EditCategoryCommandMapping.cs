using Sportswear.Core.Features.Category.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.CategoryMapping
{
    public partial class CategoryProfile
    {
        public void EditCategoryCommandMapping()
        {
            CreateMap<EditCategoryCommand, Category>();
        }
    }
}
