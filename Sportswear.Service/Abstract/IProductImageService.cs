using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IProductImageService
    {
        public Task<List<ProductImage>> GetProduct_ImagesByProductIdAsync(int productId);
        public Task<ProductImage> GetImageByProductIdAndImageUrlAsync(int productId, string imageUrl);
        public Task<bool> AddAsync(ProductImage productImage);
        public Task<bool> AddRangeAsync(ICollection<ProductImage> productImages);
        public Task<bool> EditAsync(ProductImage productImage);
        public Task<bool> EditRangeAsync(ICollection<ProductImage> productImages);
        public Task<bool> DeleteAsync(ProductImage productImage);
        public Task<bool> DeleteRangeAsync(ICollection<ProductImage> productImages);
    }
}
