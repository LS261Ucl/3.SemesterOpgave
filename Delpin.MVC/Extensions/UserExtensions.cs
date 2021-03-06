using System.Security.Claims;

namespace Delpin.Mvc.Extensions
{
    // Extensions for the ClaimsPrincipal (HttpContext.User) class to make it easier to access certain Claims

    public static class UserExtensions
    {
        public static string GetToken(this ClaimsPrincipal user)
        {
            return user.FindFirstValue("Token");
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) == "Admin";
        }

        public static bool IsSuperUser(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) == "SuperUser";
        }
    }
}