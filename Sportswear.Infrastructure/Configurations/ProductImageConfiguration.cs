using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.Property(pi => pi.Url)
                   .IsRequired();

            builder.HasOne(pi => pi.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(pi => !pi.IsDeleted);
        }
    }
}
