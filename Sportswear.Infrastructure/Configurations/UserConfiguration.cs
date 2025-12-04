using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Set index on email to enhance search and make unique
            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reviews)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserRefreshTokens)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Carts)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
