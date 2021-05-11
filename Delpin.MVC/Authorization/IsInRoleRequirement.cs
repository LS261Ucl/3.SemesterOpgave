using Microsoft.AspNetCore.Authorization;

namespace Delpin.Mvc.Authorization
{
    public class IsInRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; set; }

        public IsInRoleRequirement(string role)
        {
            Role = role;
        }
    }
}