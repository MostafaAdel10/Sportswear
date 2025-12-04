namespace Sportswear.Core.Features.CartItem.Queries.Response_DTO_
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }

        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; } // السعر بعد الخصم
        public int Quantity { get; set; }
    }
}
