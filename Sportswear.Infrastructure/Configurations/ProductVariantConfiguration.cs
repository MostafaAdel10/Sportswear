using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            // Unique variant per product (Product + Color + Size)
            builder.HasIndex(x => new { x.ProductId, x.ColorName, x.Size })
                   .IsUnique();

            // Unique SKU
            builder.HasIndex(x => x.SKU)
                   .IsUnique();

            builder.HasOne(pv => pv.Product)
                   .WithMany(p => p.Variants)
                   .HasForeignKey(pv => pv.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(pv => pv.CartItems)
                   .WithOne(ci => ci.ProductVariant)
                   .HasForeignKey(ci => ci.ProductVariantId)
                   .IsRequired(false)          // ✅ مهم جدًا
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(v => !v.IsDeleted);
        }
    }
}
