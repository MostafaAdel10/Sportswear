using MediatR;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;
using Sportswear.Core.Wrappers;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductPaginatedListQuery : IRequest<PaginatedResult<GetProductPaginatedListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public ProductOrderingEnum Ordering { get; set; } = ProductOrderingEnum.Id;
    }
}
