using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.Items)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.ProductVariant)
                   .WithMany(pv => pv.CartItems)
                   .HasForeignKey(ci => ci.ProductVariantId)
                   .IsRequired(false)            // ✅ مهم جدًا
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
