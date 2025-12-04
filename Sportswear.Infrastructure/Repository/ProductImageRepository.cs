using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ProductImageRepository : GenericRepositoryAsync<ProductImage>, IProductImageRepository
    {
        #region Fields
        private readonly DbSet<ProductImage> _productImages;
        #endregion

        #region Contractors
        public ProductImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _productImages = dbContext.Set<ProductImage>();
        }
        #endregion

        #region Handle Functions

        public async Task<ProductImage> GetImageByProductIdAndImageUrlAsync(int productId, string imageUrl)
        {
            return await GetTableAsTracking().Where(x => x.ProductId == productId && x.Url == imageUrl).FirstOrDefaultAsync();
        }

        public async Task<List<ProductImage>> GetProduct_ImagesByProductIdAsync(int productId)
        {
            return await GetTableAsTracking().Where(x => x.ProductId == productId).ToListAsync();
        }
        #endregion
    }
}
