using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IProduct_DiscountRepository : IGenericRepositoryAsync<Product_Discount>
    {
        public Task<bool> ExistsAsync(int discountId, int productId); // التحقق من وجود رابط
        public Task<List<Product_Discount>> GetByDiscountIdAsync(int discountId); // للـ Update
        public Task<List<Product_Discount>> GetByDiscountAndProductsAsync(int discountId, List<int> productIds); // للـ Remove
    }
}
