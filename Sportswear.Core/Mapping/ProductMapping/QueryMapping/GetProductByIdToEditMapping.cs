using Sportswear.Core.Features.Product.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ProductMapping
{
    public partial class ProductProfile
    {
        public void GetProductByIdToEditMapping()
        {
            CreateMap<Product, GetProductByIdToEditResponse>();
        }
    }
}
