using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ShippingMethodService : IShippingMethodService
    {
        #region Fields 
        private readonly IShippingMethodRepository _shippingMethodRepository;
        #endregion

        #region Contractors
        public ShippingMethodService(IShippingMethodRepository shippingMethodRepository)
        {
            _shippingMethodRepository = shippingMethodRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(ShippingMethod shippingMethod)
        {
            await _shippingMethodRepository.AddAsync(shippingMethod);
            return true;
        }

        public async Task<bool> DeleteAsync(ShippingMethod shippingMethod)
        {
            var transaction = _shippingMethodRepository.BeginTransaction();

            try
            {
                await _shippingMethodRepository.DeleteAsync(shippingMethod);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(ShippingMethod shippingMethod)
        {
            await _shippingMethodRepository.UpdateAsync(shippingMethod);
            return true;
        }

        public async Task<ShippingMethod> GetByIdAsync(int shippingMethodId)
        {
            var shippingMethod = await _shippingMethodRepository.GetByIdAsync(shippingMethodId);
            return shippingMethod;
        }

        public async Task<List<ShippingMethod>> GetShippingMethodsListAsync()
        {
            return await _shippingMethodRepository.GetShippingMethodsListAsync();
        }
        #endregion
    }
}
