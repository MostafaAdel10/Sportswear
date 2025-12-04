using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class DiscountService : IDiscountService
    {
        #region Fields 
        private readonly IDiscountRepository _discountRepository;
        #endregion

        #region Contractors
        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(Discount discount)
        {
            await _discountRepository.AddAsync(discount);
            return true;
        }

        public async Task<bool> DeleteAsync(Discount discount)
        {
            var transaction = _discountRepository.BeginTransaction();

            try
            {
                await _discountRepository.DeleteAsync(discount);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(Discount discount)
        {
            await _discountRepository.UpdateAsync(discount);
            return true;
        }

        public async Task<Discount?> GetActiveDiscountByIdAsync(int discountId)
        {
            var discount = await _discountRepository.GetActiveDiscountByIdAsync(discountId);
            return discount;
        }

        public async Task<List<Discount>> GetActiveDiscountsAsync()
        {
            return await _discountRepository.GetActiveDiscountsAsync();
        }

        public async Task<Discount> GetByIdAsync(int discountId)
        {
            var brand = await _discountRepository.GetByIdAsync(discountId);
            return brand;
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            return await _discountRepository.IsCodeExistsAsync(code);
        }

        public async Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id)
        {
            return await _discountRepository.IsCodeExistsExcludeSelfAsync(code, id);
        }

        public async Task<bool> IsDiscountIdExist(int discountId)
        {
            return await _discountRepository.IsDiscountIdExist(discountId);
        }
        #endregion
    }
}
