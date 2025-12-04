using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IOrderItemRepository : IGenericRepositoryAsync<OrderItem>
    {
    }
}
