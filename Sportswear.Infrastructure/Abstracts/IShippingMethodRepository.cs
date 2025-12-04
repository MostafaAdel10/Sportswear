using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IShippingMethodRepository : IGenericRepositoryAsync<ShippingMethod>
    {
        public Task<List<ShippingMethod>> GetShippingMethodsListAsync();
    }
}
