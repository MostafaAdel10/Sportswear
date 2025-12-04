using Sportswear.Core.Features.Brand.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.BrandMapping
{
    public partial class BrandProfile
    {
        public void EditBrandCommandMapping()
        {
            CreateMap<EditBrandCommand, Brand>();
        }
    }
}
