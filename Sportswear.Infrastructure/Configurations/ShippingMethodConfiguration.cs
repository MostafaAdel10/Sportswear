using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class ShippingMethodConfiguration : IEntityTypeConfiguration<ShippingMethod>
    {
        public void Configure(EntityTypeBuilder<ShippingMethod> builder)
        {
            builder.HasMany(sm => sm.Shipments)
                   .WithOne(s => s.ShippingMethod)
                   .HasForeignKey(s => s.ShippingMethodId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
