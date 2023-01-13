using System.Security.Claims;

namespace MiniArmory.Web.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static Guid GetId(this ClaimsPrincipal claims)
            => Guid.Parse(claims.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
