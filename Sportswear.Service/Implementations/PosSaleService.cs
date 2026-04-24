using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class PosSaleService : IPosSaleService
    {
        private readonly IPosSaleRepository _posSaleRepository;

        public PosSaleService(IPosSaleRepository posSaleRepository)
        {
            _posSaleRepository = posSaleRepository;
        }

        public async Task<PosSale?> GetByIdWithItemsAsync(int id)
        {
            return await _posSaleRepository.GetByIdWithItemsAsync(id);
        }

        public async Task<List<PosSale>> GetAllWithItemsAsync()
        {
            return await _posSaleRepository.GetAllWithItemsAsync();
        }

        public async Task<int> AddAsync(PosSale posSale)
        {
            var result = await _posSaleRepository.AddAsync(posSale);
            return result.Id;
        }

        public async Task<bool> EditAsync(PosSale posSale)
        {
            await _posSaleRepository.UpdateAsync(posSale);
            return true;
        }

        public async Task<string> GenerateSaleNumberAsync()
        {
            return await _posSaleRepository.GenerateSaleNumberAsync();
        }

        public async Task<List<PosSale>> GetPosSalesForDashboardAsync(DateTime from)
        {
            return await _posSaleRepository.GetPosSalesForDashboardAsync(from);
        }
    }
}
