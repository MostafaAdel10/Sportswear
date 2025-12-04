using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        #region Fields
        private readonly DbSet<Payment> _payments;
        #endregion

        #region Contractors
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _payments = dbContext.Set<Payment>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
