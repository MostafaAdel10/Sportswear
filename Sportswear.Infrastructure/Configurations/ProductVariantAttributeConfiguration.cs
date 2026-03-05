using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class ProductVariantAttributeConfiguration : IEntityTypeConfiguration<ProductVariantAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductVariantAttribute> builder)
        {
            // مش ممكن نفس الـ Variant يبقى عنده نفس الـ Template اتنين مرات
            builder.HasIndex(x => new { x.ProductVariantId, x.ProductAttributeTemplateId })
                   .HasFilter("[IsDeleted] = 0")
                   .IsUnique();

            builder.HasOne(a => a.ProductVariant)
                   .WithMany(pv => pv.Attributes)
                   .HasForeignKey(a => a.ProductVariantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.ProductAttributeTemplate)
                   .WithMany(t => t.VariantAttributes)
                   .HasForeignKey(a => a.ProductAttributeTemplateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
