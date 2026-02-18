using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ProductVariantService : IProductVariantService
    {
        #region Fields 
        private readonly IProductVariantRepository _productVariantRepository;
        #endregion

        #region Contractors
        public ProductVariantService(IProductVariantRepository productVariantRepository)
        {
            _productVariantRepository = productVariantRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddRangeAsync(List<ProductVariant> variants)
        {
            await _productVariantRepository.AddRangeAsync(variants);
            return true;
        }

        public async Task<bool> AddAsync(ProductVariant productVariant)
        {
            await _productVariantRepository.AddAsync(productVariant);
            return true;
        }

        public async Task<bool> DeleteAsync(ProductVariant productVariant)
        {
            var transaction = _productVariantRepository.BeginTransaction();

            try
            {
                await _productVariantRepository.DeleteAsync(productVariant);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(ProductVariant productVariant)
        {
            await _productVariantRepository.UpdateAsync(productVariant);
            return true;
        }

        public async Task<List<ProductVariant>> GetByProductIdAsync(int productId)
        {
            var productVariants = await _productVariantRepository.GetByProductIdAsync(productId);
            return productVariants;
        }

        public async Task<ProductVariant> GetByIdAsync(int id)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(id);
            return productVariant;
        }

        public async Task<ProductVariant> GetByIdWithIncludesAsync(int id)
        {
            var productVariant = await _productVariantRepository.GetByIdWithIncludesAsync(id);
            return productVariant;
        }

        public async Task<bool> IsProductVariantExistsAsync(int productVariantId)
        {
            return await _productVariantRepository.IsProductVariantExistsAsync(productVariantId);
        }

        public async Task<bool> IsProductVariantExistsExcludeSelfAsync(int productVariantId, int id)
        {
            return await _productVariantRepository.IsProductVariantExistsExcludeSelfAsync(productVariantId, id);
        }

        public async Task<bool> ExistsAsync(int productId, string colorName, string size, int excludeId)
        {
            return await _productVariantRepository.ExistsAsync(productId, colorName, size, excludeId);
        }

        #endregion
    }
}
