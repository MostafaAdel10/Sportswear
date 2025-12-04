using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductsListQuery : IRequest<Response<List<GetProductsListResponse>>>
    {
    }
}
