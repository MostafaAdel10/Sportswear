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

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
