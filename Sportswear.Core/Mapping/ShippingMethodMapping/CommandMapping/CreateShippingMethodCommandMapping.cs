using AutoMapper;
using Sportswear.Core.Features.ShippingMethod.Commands.Models;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Core.Mapping.ShippingMethodMapping
{
    public partial class ShippingMethodProfile : Profile
    {
        public void CreateShippingMethodCommandMapping()
        {
            CreateMap<CreateShippingMethodCommand, ShippingMethod>();
        }
    }
}
