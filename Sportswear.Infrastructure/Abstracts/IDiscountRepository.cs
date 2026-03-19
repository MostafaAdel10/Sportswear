using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IDiscountRepository : IGenericRepositoryAsync<Discount>
    {
        public Task<List<Discount>> GetExpiredDiscountsAsync(DateTime now);

        public Task<List<Discount>> GetAllWithProductsCountAsync();
        public Task<bool> IsDiscountIdExist(int discountId);
        public Task<List<Discount>> GetActiveDiscountsAsync();
        public Task<Discount?> GetActiveDiscountByIdAsync(int discountId);
        public Task<bool> IsCodeExistsAsync(string code);
        public Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id);
    }
}
