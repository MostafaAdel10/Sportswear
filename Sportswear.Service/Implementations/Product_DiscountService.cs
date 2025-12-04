using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class Product_DiscountService : IProduct_DiscountService
    {
        #region Fields
        private readonly IProduct_DiscountRepository _product_DiscountRepository;
        #endregion

        #region Contractors
        public Product_DiscountService(IProduct_DiscountRepository product_DiscountRepository)
        {
            _product_DiscountRepository = product_DiscountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddRangeAsync(List<Product_Discount> product_Discounts)
        {
            var transaction = _product_DiscountRepository.BeginTransaction();

            try
            {
                await _product_DiscountRepository.AddRangeAsync(product_Discounts);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteProductDiscountsAsync(Product_Discount productDiscount)
        {
            var transaction = _product_DiscountRepository.BeginTransaction();

            try
            {
                await _product_DiscountRepository.DeleteAsync(productDiscount);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<Product_Discount> product_Discounts)
        {
            var transaction = _product_DiscountRepository.BeginTransaction();

            try
            {
                await _product_DiscountRepository.DeleteRangeAsync(product_Discounts);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ExistsAsync(int discountId, int productId)
        {
            return await _product_DiscountRepository.ExistsAsync(discountId, productId);
        }

        public async Task<List<Product_Discount>> GetByDiscountAndProductsAsync(int discountId, List<int> productIds)
        {
            return await _product_DiscountRepository.GetByDiscountAndProductsAsync(discountId, productIds);
        }

        public async Task<List<Product_Discount>> GetByDiscountIdAsync(int discountId)
        {
            return await _product_DiscountRepository.GetByDiscountIdAsync(discountId);
        }

        public async Task<Product_Discount> GetByIdAsync(int id)
        {
            var product_Discount = await _product_DiscountRepository.GetByIdAsync(id);
            return product_Discount;
        }
        #endregion
    }
}
