using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .IsRequired(false)              // ✅ بسبب QueryFilter على Order
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.ProductVariant)
                   .WithMany(pv => pv.OrderItems)
                   .HasForeignKey(oi => oi.ProductVariantId)
                   .IsRequired(false)              // ✅ بسبب QueryFilter على ProductVariant
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
