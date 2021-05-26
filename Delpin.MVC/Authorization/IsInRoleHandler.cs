using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Delpin.Mvc.Authorization
{
    // Handles the authorization request where it checks the users role against the requirement

    public class IsInRoleHandler : AuthorizationHandler<IsInRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsInRoleRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "Token"))
                return Task.CompletedTask;

            var userRole = context.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            if (requirement.Role == userRole || userRole == "Admin")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}