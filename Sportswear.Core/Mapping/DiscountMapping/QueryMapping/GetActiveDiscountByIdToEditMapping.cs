using Sportswear.Core.Features.Discount.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.DiscountMapping
{
    public partial class DiscountProfile
    {
        public void GetActiveDiscountByIdToEditMapping()
        {
            CreateMap<Discount, GetDiscountByIdToEditResponse>();
        }
    }
}
