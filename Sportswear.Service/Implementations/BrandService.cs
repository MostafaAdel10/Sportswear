using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class BrandService : IBrandService
    {
        #region Fields 
        private readonly IBrandRepository _brandRepository;
        #endregion

        #region Contractors
        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> IsBrandIdExist(int brandId)
        {
            return await _brandRepository.IsBrandIdExist(brandId);
        }

        public async Task<List<Brand>> GetBrandsListAsync()
        {
            return await _brandRepository.GetBrandsListAsync();
        }

        public async Task<Brand> GetByIdAsync(int brandId)
        {
            var brand = await _brandRepository.GetByIdAsync(brandId);
            return brand;
        }

        public async Task<bool> AddAsync(Brand brand)
        {
            await _brandRepository.AddAsync(brand);
            return true;
        }

        public async Task<bool> EditAsync(Brand brand)
        {
            await _brandRepository.UpdateAsync(brand);
            return true;
        }

        public async Task<bool> DeleteAsync(Brand brand)
        {
            var transaction = _brandRepository.BeginTransaction();

            try
            {
                await _brandRepository.DeleteAsync(brand);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        #endregion
    }
}
