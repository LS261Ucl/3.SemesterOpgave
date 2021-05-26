using Microsoft.AspNetCore.Authorization;

namespace Delpin.Mvc.Authorization
{
    // Holds the role of a user in a string
    public class IsInRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; set; }

        public IsInRoleRequirement(string role)
        {
            Role = role;
        }
    }
}