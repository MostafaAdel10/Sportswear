using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;

namespace Sportswear.Core.Features.Product.Queries.Models
{
    public class GetProductByCodeWithVariantsQuery : IRequest<Response<GetProductByCodeWithVariantsResponse>>
    {
        public GetProductByCodeWithVariantsQuery(string code)
        {
            Code = code;
        }
        public string Code { get; set; }
    }
}
