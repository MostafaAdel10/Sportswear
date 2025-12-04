using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ProductImageService : IProductImageService
    {
        #region Fields 
        private readonly IProductImageRepository _productImageRepository;
        #endregion

        #region Contractors
        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(ProductImage productImage)
        {
            await _productImageRepository.AddAsync(productImage);
            return true;
        }

        public async Task<bool> AddRangeAsync(ICollection<ProductImage> productImages)
        {
            var transaction = _productImageRepository.BeginTransaction();

            try
            {
                await _productImageRepository.AddRangeAsync(productImages);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ProductImage productImage)
        {
            var transaction = _productImageRepository.BeginTransaction();

            try
            {
                await _productImageRepository.DeleteAsync(productImage);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(ICollection<ProductImage> productImages)
        {
            var transaction = _productImageRepository.BeginTransaction();

            try
            {
                await _productImageRepository.DeleteRangeAsync(productImages);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(ProductImage productImage)
        {
            await _productImageRepository.UpdateAsync(productImage);
            return true;
        }

        public async Task<bool> EditRangeAsync(ICollection<ProductImage> productImages)
        {
            var transaction = _productImageRepository.BeginTransaction();

            try
            {
                await _productImageRepository.UpdateRangeAsync(productImages);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<ProductImage> GetImageByProductIdAndImageUrlAsync(int productId, string imageUrl)
        {
            return await _productImageRepository.GetImageByProductIdAndImageUrlAsync(productId, imageUrl);
        }

        public async Task<List<ProductImage>> GetProduct_ImagesByProductIdAsync(int productId)
        {
            return await _productImageRepository.GetProduct_ImagesByProductIdAsync(productId);
        }
        #endregion
    }
}
