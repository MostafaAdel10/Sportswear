using MediatR;
using Sportswear.Core.Bases;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Core.Features.Discount.Commands.Models
{
    public class CreateDiscountCommand : IRequest<Response<string>>
    {
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType Type { get; set; }
    }
}
