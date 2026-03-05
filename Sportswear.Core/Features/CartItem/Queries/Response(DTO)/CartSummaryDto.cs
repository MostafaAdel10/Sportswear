namespace Sportswear.Core.Features.CartItem.Queries.Response_DTO_
{
    public class CartSummaryDto
    {
        public int TotalItems { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
    }
}
