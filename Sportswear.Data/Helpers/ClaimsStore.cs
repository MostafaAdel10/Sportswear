using System.Security.Claims;

namespace Sportswear.DataAccess.Helpers
{
    public class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
            //new Claim("Create Product","false"),
            //new Claim("Edit Product","false"),
            //new Claim("Delete Product","false"),
        };
    }
}
