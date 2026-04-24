using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IPosSaleRepository : IGenericRepositoryAsync<PosSale>
    {
        public Task<PosSale?> GetByIdWithItemsAsync(int id);
        public Task<List<PosSale>> GetAllWithItemsAsync();
        public Task<string> GenerateSaleNumberAsync();
        public Task<List<PosSale>> GetPosSalesForDashboardAsync(DateTime from);
    }
}
