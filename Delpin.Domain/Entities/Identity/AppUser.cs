using Microsoft.AspNetCore.Identity;

namespace Delpin.Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}