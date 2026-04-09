using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Product.Commands.Models
{
    public class CreateProductCommand : IRequest<Response<int>>
    {
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public decimal BasePrice { get; set; }
        public string? Season { get; set; }
        public string? ClubEn { get; set; }
        public string? ClubAr { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public string? AttributeKeyEn { get; set; }
        public string? AttributeKeyAr { get; set; }
    }
}
