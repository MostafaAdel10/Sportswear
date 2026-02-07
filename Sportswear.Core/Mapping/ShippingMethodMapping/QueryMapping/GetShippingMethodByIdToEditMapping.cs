using Sportswear.Core.Features.ShippingMethod.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ShippingMethodMapping
{
    public partial class ShippingMethodProfile
    {
        public void GetShippingMethodByIdToEditMapping()
        {
            CreateMap<ShippingMethod, GetShippingMethodByIdToEditResponse>();
        }
    }
}
