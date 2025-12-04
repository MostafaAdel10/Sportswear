using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IShipmentService
    {
        public Task<bool> IsAnyShipmentRelatedToShippingMethodAsync(int shippingMethodId);
        public Task<bool> AddAsync(Shipment shipment);
        public Task<bool> EditAsync(Shipment shipment);
        public Task<bool> DeleteAsync(Shipment shipment);
    }
}
