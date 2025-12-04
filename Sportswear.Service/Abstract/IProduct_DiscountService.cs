using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IProduct_DiscountService
    {
        public Task<Product_Discount> GetByIdAsync(int id);
        public Task<bool> DeleteProductDiscountsAsync(Product_Discount productDiscount);
        public Task<bool> ExistsAsync(int discountId, int productId); // التحقق من وجود رابط
        public Task<List<Product_Discount>> GetByDiscountIdAsync(int discountId); // للـ Update
        public Task<List<Product_Discount>> GetByDiscountAndProductsAsync(int discountId, List<int> productIds); // للـ Remove
        public Task<bool> AddRangeAsync(List<Product_Discount> product_Discounts);
        public Task<bool> DeleteRangeAsync(List<Product_Discount> product_Discounts);
    }
}
