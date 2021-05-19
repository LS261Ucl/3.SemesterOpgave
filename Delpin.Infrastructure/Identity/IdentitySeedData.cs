using Delpin.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delpin.Infrastructure.Identity
{
    public class IdentitySeedData
    {
        public static async Task SeedIdentityDatabase(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        FullName = "Mikkel Buhl", UserName = "mikkel@delpin.dk", Email = "mikkel@delpin.dk"
                    },
                    new AppUser
                    {
                        FullName = "Kristoffer Kruse", UserName = "kristoffer@delpin.dk", Email = "kristoffer@delpin.dk"
                    },
                    new AppUser
                    {
                        FullName = "Lene Svit", UserName = "lene@delpin.dk", Email = "lene@delpin.dk"
                    },
                    new AppUser
                    {
                        FullName = "Kirstine Jensen", UserName = "kirstine@delpin.dk", Email = "kirstine@delpin.dk"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }
    }
}