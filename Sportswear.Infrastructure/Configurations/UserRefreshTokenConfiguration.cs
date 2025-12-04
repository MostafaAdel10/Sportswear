using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Infrastructure.Configurations
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.UserRefreshTokens)
                   .HasForeignKey(rt => rt.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
