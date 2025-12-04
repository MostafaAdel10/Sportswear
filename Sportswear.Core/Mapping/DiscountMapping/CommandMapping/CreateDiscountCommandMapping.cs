using Sportswear.Core.Features.Discount.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.DiscountMapping
{
    public partial class DiscountProfile
    {
        public void CreateDiscountCommandMapping()
        {
            CreateMap<CreateDiscountCommand, Discount>();
        }
    }
}
