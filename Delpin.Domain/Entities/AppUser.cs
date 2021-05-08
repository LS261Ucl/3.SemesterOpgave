using Microsoft.AspNetCore.Identity;

namespace Delpin.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}