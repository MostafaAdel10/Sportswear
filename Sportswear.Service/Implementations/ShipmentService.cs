using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ShipmentService : IShipmentService
    {
        #region Fields 
        private readonly IShipmentRepository _shipmentRepository;
        #endregion

        #region Contractors
        public ShipmentService(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(Shipment shipment)
        {
            await _shipmentRepository.AddAsync(shipment);
            return true;
        }

        public async Task<bool> DeleteAsync(Shipment shipment)
        {
            var transaction = _shipmentRepository.BeginTransaction();

            try
            {
                await _shipmentRepository.DeleteAsync(shipment);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(Shipment shipment)
        {
            await _shipmentRepository.UpdateAsync(shipment);
            return true;
        }

        public async Task<bool> IsAnyShipmentRelatedToShippingMethodAsync(int shippingMethodId)
        {
            return await _shipmentRepository.IsAnyShipmentRelatedToShippingMethodAsync(shippingMethodId);
        }
        #endregion
    }
}
