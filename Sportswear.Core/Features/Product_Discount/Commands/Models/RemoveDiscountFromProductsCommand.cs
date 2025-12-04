using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Product_Discount.Commands.Models
{
    public class RemoveDiscountFromProductsCommand : IRequest<Response<string>>
    {
        public int DiscountId { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>(); // إذا فارغ، حذف من كل المنتجات
    }
}
