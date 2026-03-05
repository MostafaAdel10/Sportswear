using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasMany(d => d.Product_Discounts)
                   .WithOne(pd => pd.Discount)
                   .HasForeignKey(pd => pd.DiscountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(d => d.Percentage)   // ✅ حل تحذير decimal
                   .HasPrecision(5, 2);

            builder.HasQueryFilter(d => !d.IsDeleted);
        }
    }
}
