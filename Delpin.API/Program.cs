using Delpin.Domain.Entities.Identity;
using Delpin.Infrastructure.Data;
using Delpin.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Delpin.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // Makes a extern scope to seed and migrate data before startup is run
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DelpinContext>();
                    context.Database.Migrate();
                    SeedData.SeedDatabase(context);
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "Error occured seeding the Database");
                }

                try
                {
                    var identityContext = services.GetRequiredService<DelpinIdentityContext>();
                    var userManger = services.GetRequiredService<UserManager<AppUser>>();
                    identityContext.Database.Migrate();
                    IdentitySeedData.SeedIdentityDatabase(userManger).Wait();
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "Error occured seeding the Identity Database");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}