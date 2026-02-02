using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class Product_DiscountConfiguration : IEntityTypeConfiguration<Product_Discount>
    {
        public void Configure(EntityTypeBuilder<Product_Discount> builder)
        {
            builder.HasKey(pd => new { pd.ProductId, pd.DiscountId });

            builder.HasOne(pd => pd.Product)
                   .WithMany(p => p.Product_Discounts)
                   .HasForeignKey(pd => pd.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pd => pd.Discount)
                   .WithMany(d => d.Product_Discounts)
                   .HasForeignKey(pd => pd.DiscountId)
                   .IsRequired(false)          // ✅ مهم جدًا
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
