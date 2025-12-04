using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Product_Discount.Commands.Models
{
    public class UpdateProductsForDiscountCommand : IRequest<Response<string>>
    {
        public int DiscountId { get; set; }
        public List<int> NewProductIds { get; set; } = new List<int>(); // المنتجات الجديدة المراد ربطها
    }
}
