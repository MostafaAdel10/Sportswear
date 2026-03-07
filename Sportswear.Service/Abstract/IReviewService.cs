using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IReviewService
    {
        public Task<Review> GetByIdAsync(int reviewId);
        public Task<bool> AddAsync(Review review);
        public Task<bool> EditAsync(Review review);
        public Task<bool> DeleteAsync(Review review);
        public Task<List<Review>> GetReviewsByProductIdAsync(int productId);
        public Task<Review?> GetByIdWithIncludesAsync(int id);

    }
}
