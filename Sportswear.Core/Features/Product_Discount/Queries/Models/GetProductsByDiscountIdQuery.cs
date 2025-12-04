using MediatR;
using Sportswear.Core.Features.Product_Discount.Queries.Response_DTO_;
using Sportswear.Core.Wrappers;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.Product_Discount.Queries.Models
{
    public class GetProductsByDiscountIdQuery : IRequest<PaginatedResult<GetProductsByDiscountIdResponse>>
    {
        public int DiscountId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public ProductOrderingEnum Ordering { get; set; } = ProductOrderingEnum.Id;
    }
}
