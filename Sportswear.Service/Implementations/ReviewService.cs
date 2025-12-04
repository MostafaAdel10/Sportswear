using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ReviewService : IReviewService
    {
        #region Fields 
        private readonly IReviewRepository _reviewRepository;
        #endregion

        #region Contractors
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(Review review)
        {
            await _reviewRepository.AddAsync(review);
            return true;
        }

        public async Task<bool> DeleteAsync(Review review)
        {
            var transaction = _reviewRepository.BeginTransaction();

            try
            {
                await _reviewRepository.DeleteAsync(review);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(Review review)
        {
            await _reviewRepository.UpdateAsync(review);
            return true;
        }

        public async Task<Review> GetByIdAsync(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            return review;
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _reviewRepository.GetReviewsByProductIdAsync(productId);
        }
        #endregion
    }
}
