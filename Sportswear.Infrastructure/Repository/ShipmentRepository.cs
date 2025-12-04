using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ShipmentRepository : GenericRepositoryAsync<Shipment>, IShipmentRepository
    {
        #region Fields
        private readonly DbSet<Shipment> _shipments;
        #endregion

        #region Contractors
        public ShipmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _shipments = dbContext.Set<Shipment>();
        }
        #endregion

        #region Handle Functions
        public async Task<bool> IsAnyShipmentRelatedToShippingMethodAsync(int shippingMethodId)
        {
            return await _shipments.AnyAsync(s => s.ShippingMethodId == shippingMethodId);
        }
        #endregion
    }
}
