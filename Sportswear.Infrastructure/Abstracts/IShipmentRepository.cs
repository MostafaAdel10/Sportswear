using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IShipmentRepository : IGenericRepositoryAsync<Shipment>
    {
        public Task<bool> IsAnyShipmentRelatedToShippingMethodAsync(int shippingMethodId);
    }
}
