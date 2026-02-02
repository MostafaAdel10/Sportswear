using System.Security.Claims;

namespace Sportswear.DataAccess.Helpers
{
    public class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
            new Claim("Create Product","true"),
            new Claim("Edit Product","true"),
            new Claim("Delete Product","true"),
        };
    }
}
