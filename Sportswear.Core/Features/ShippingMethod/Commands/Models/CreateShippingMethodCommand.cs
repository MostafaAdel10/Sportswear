using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ShippingMethod.Commands.Models
{
    public class CreateShippingMethodCommand : IRequest<Response<string>>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
    }
}
