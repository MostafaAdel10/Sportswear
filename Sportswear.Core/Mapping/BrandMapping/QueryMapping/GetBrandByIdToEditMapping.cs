using Sportswear.Core.Features.Brand.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.BrandMapping
{
    public partial class BrandProfile
    {
        public void GetBrandByIdToEditMapping()
        {
            CreateMap<Brand, GetBrandByIdToEditResponse>();
        }
    }
}
