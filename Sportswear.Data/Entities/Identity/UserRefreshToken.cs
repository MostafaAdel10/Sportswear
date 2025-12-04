using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities.Identity
{
    public class UserRefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationUserId { get; set; }

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ExpiryDate { get; set; }


        [ForeignKey(nameof(ApplicationUserId))]
        [InverseProperty(nameof(ApplicationUser.UserRefreshTokens))]
        public virtual ApplicationUser? User { get; set; }
    }
}
