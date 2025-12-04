using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ReviewRepository : GenericRepositoryAsync<Review>, IReviewRepository
    {
        #region Fields
        private readonly DbSet<Review> _reviews;
        #endregion

        #region Contractors
        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _reviews = dbContext.Set<Review>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .ToListAsync();
        }
        #endregion
    }
}
