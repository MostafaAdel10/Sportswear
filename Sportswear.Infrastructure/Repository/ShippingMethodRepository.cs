using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ShippingMethodRepository : GenericRepositoryAsync<ShippingMethod>, IShippingMethodRepository
    {
        #region Fields
        private readonly DbSet<ShippingMethod> _shippingMethods;
        #endregion

        #region Contractors
        public ShippingMethodRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _shippingMethods = dbContext.Set<ShippingMethod>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<ShippingMethod>> GetShippingMethodsListAsync()
        {
            return await _shippingMethods.ToListAsync();
        }
        #endregion
    }
}
