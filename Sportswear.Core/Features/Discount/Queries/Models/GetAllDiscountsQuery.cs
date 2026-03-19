using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.Discount.Queries.Models
{
    public class GetAllDiscountsQuery : IRequest<Response<List<DiscountDto>>>
    {
        public DiscountStatusFilter Status { get; set; } = DiscountStatusFilter.All;
    }
}
