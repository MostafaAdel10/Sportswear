using AutoMapper;

namespace Sportswear.Core.Mapping.ShippingMethodMapping
{
    public partial class ShippingMethodProfile : Profile
    {
        public ShippingMethodProfile()
        {
            CreateShippingMethodCommandMapping();
            EditShippingMethodCommandMapping();
            GetShippingMethodByIdMapping();
            GetShippingMethodsListMapping();
            GetShippingMethodByIdToEditMapping();
        }
    }
}
