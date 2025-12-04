using Sportswear.Core.Features.Product.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ProductMapping
{
    public partial class ProductProfile
    {
        public void CreateProductCommandMapping()
        {
            CreateMap<CreateProductCommand, Product>();
        }
    }
}
