using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IDiscountService
    {
        public Task<List<Discount>> GetActiveDiscountsAsync();
        public Task<Discount?> GetActiveDiscountByIdAsync(int discountId);
        public Task<bool> IsDiscountIdExist(int discountId);
        public Task<Discount> GetByIdAsync(int discountId);
        public Task<bool> AddAsync(Discount discount);
        public Task<bool> EditAsync(Discount discount);
        public Task<bool> DeleteAsync(Discount discount);
        public Task<bool> IsCodeExistsAsync(string code);
        public Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id);
    }
}
