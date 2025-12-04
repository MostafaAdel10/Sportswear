using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IShippingMethodService
    {
        public Task<List<ShippingMethod>> GetShippingMethodsListAsync();
        public Task<ShippingMethod> GetByIdAsync(int shippingMethodId);
        public Task<bool> AddAsync(ShippingMethod shippingMethod);
        public Task<bool> EditAsync(ShippingMethod shippingMethod);
        public Task<bool> DeleteAsync(ShippingMethod shippingMethod);
    }
}
