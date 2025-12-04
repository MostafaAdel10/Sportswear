using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Product_Discount.Commands.Models
{
    public class AddDiscountToProductsCommand : IRequest<Response<string>>
    {
        public int DiscountId { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>(); // لدعم الإضافة المتعددة
    }
}
