using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasOne(s => s.ShippingMethod)
                   .WithMany(m => m.Shipments)
                   .HasForeignKey(s => s.ShippingMethodId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Order)
                   .WithOne(o => o.Shipment)
                   .HasForeignKey<Shipment>(s => s.OrderId);
        }
    }
}
