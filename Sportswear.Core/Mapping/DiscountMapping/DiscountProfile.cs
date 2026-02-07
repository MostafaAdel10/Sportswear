using AutoMapper;

namespace Sportswear.Core.Mapping.DiscountMapping
{
    public partial class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            GetActiveDiscountsListMapping();
            GetActiveDiscountByIdMapping();
            CreateDiscountCommandMapping();
            EditDiscountCommandMapping();
            GetActiveDiscountByIdToEditMapping();
        }
    }
}
