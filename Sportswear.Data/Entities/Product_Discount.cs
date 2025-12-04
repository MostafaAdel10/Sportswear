using System.ComponentModel.DataAnnotations;

namespace Sportswear.DataAccess.Entities
{
    public class Product_Discount
    {
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
