using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IProductImageRepository : IGenericRepositoryAsync<ProductImage>
    {
        public Task<List<ProductImage>> GetProduct_ImagesByProductIdAsync(int productId);
        public Task<ProductImage> GetImageByProductIdAndImageUrlAsync(int productId, string imageUrl);
    }
}
