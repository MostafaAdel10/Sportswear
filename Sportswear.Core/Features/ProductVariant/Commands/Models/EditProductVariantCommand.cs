using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.ProductVariant.Commands.Models
{
    public class EditProductVariantCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ProductId { get; set; }
    }
}
