using System.Security.Claims;

namespace Delpin.Mvc.Extensions
{
    public static class UserExtensions
    {
        public static string GetToken(this ClaimsPrincipal user)
        {
            return user.FindFirstValue("Token");
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role);
        }
    }
}