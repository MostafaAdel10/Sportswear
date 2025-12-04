using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportswear.DataAccess.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            Orders = new List<Order>();
            Reviews = new List<Review>();
            Carts = new List<Cart>();
            UserRefreshTokens = new HashSet<UserRefreshToken>();
        }
        public DateTime? BirthDate { get; set; }

        [EncryptColumn]
        [MaxLength(100)]
        public string? Code { get; set; }


        // Navigation Property
        [InverseProperty(nameof(UserRefreshToken.User))]
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Cart> Carts { get; set; }

    }
}
