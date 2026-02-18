using Sportswear.Core.Features.ProductVariant.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ProductVariantMapping
{
    public partial class ProductVariantProfile
    {
        public void GetProductVariantByIdToEditMapping()
        {
            CreateMap<ProductVariant, GetProductVariantByIdToEditResponse>();
        }
    }
}
