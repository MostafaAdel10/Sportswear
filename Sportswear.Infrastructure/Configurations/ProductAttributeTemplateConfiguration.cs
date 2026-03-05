using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities;

namespace Sportswear.Infrastructure.Configurations
{
    public class ProductAttributeTemplateConfiguration : IEntityTypeConfiguration<ProductAttributeTemplate>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeTemplate> builder)
        {
            // مش ممكن يبقى في نفس الـ Category اتنين attributes بنفس الاسم
            builder.HasIndex(x => new { x.CategoryId, x.KeyEn })
                   .HasFilter("[IsDeleted] = 0")
                   .IsUnique();

            builder.HasOne(t => t.Category)
                   .WithMany(c => c.AttributeTemplates)
                   .HasForeignKey(t => t.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.VariantAttributes)
                   .WithOne(a => a.ProductAttributeTemplate)
                   .HasForeignKey(a => a.ProductAttributeTemplateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
