using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IReviewRepository : IGenericRepositoryAsync<Review>
    {
        public Task<List<Review>> GetReviewsByProductIdAsync(int productId);
    }
}
