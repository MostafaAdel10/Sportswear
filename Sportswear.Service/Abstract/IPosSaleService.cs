using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IPosSaleService
    {
        public Task<PosSale?> GetByIdWithItemsAsync(int id);
        public Task<List<PosSale>> GetAllWithItemsAsync();
        public Task<int> AddAsync(PosSale posSale);
        public Task<bool> EditAsync(PosSale posSale);
        public Task<string> GenerateSaleNumberAsync();
    }
}
