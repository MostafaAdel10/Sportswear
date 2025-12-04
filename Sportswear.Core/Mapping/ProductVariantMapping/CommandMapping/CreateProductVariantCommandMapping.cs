using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ProductVariantMapping
{
    public partial class ProductVariantProfile
    {
        public void CreateProductVariantCommandMapping()
        {
            CreateMap<CreateProductVariantCommand, ProductVariant>();
        }
    }
}
