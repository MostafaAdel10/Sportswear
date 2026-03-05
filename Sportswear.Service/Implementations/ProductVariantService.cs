using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ProductVariantService : IProductVariantService
    {
        #region Fields 
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        #endregion

        #region Contractors
        public ProductVariantService(IProductVariantRepository productVariantRepository, IProductRepository productRepository)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddRangeAsync(List<ProductVariant> variants)
        {
            await _productVariantRepository.AddRangeAsync(variants);

            // ✅ بعد الإضافة بيحدث MinPrice و MaxPrice و HasVariants
            if (variants.Any())
                await SyncProductPricingAsync(variants.First().ProductId);

            return true;
        }

        public async Task<bool> DeleteAsync(ProductVariant productVariant)
        {
            var transaction = _productVariantRepository.BeginTransaction();

            try
            {
                await _productVariantRepository.DeleteAsync(productVariant);

                // ✅ بعد الحذف بيحدث MinPrice و MaxPrice و HasVariants
                await SyncProductPricingAsync(productVariant.ProductId);

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

            // ✅ بعد التعديل بيحدث لو السعر اتغير
            await SyncProductPricingAsync(productVariant.ProductId);

            return true;
        }

        public async Task<bool> EditStockOnlyAsync(ProductVariant productVariant)
        {
            await _productVariantRepository.UpdateAsync(productVariant);

            return true;
        }

        public async Task<List<ProductVariant>> GetByProductIdAsync(int productId)
        {
            var productVariants = await _productVariantRepository.GetByProductIdAsync(productId);
            return productVariants;
        }

        public async Task<ProductVariant?> GetByIdAsync(int id)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(id);
            return productVariant;
        }

        public async Task<ProductVariant?> GetByIdWithIncludesAsync(int id)
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
        public async Task<HashSet<string>> GetVariantKeysAsync(int productId, int excludeId = 0)
        {
            return await _productVariantRepository.GetVariantKeysAsync(productId, excludeId);
        }
        public async Task SyncProductPricingAsync(int productId)
        {
            // جيب المنتج
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return;

            // جيب كل الـ variants بتاعته
            var variants = await _productVariantRepository.GetByProductIdAsync(productId);

            if (variants.Any())
            {
                product.HasVariants = true;
                product.MinPrice = variants.Min(v => v.Price);
                product.MaxPrice = variants.Max(v => v.Price);
            }
            else
            {
                // لو مفيش variants خالص يرجع للسعر الأساسي
                product.HasVariants = false;
                product.MinPrice = product.BasePrice;
                product.MaxPrice = product.BasePrice;
            }

            await _productRepository.UpdateAsync(product);
        }
        #endregion
    }
}
