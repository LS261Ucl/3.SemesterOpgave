using Delpin.Domain.Entities;
using Delpin.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Delpin.Infrastructure.Identity
{
    public class DelpinIdentityContext : IdentityDbContext<AppUser>
    {
        public DelpinIdentityContext(DbContextOptions<DelpinIdentityContext> options) : base(options)
        {
        }
    }
}