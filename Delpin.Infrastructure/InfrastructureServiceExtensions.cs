using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Delpin.Infrastructure.Data;
using Delpin.Infrastructure.Data.Repositories;
using Delpin.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Delpin.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DelpinContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<DelpinIdentityContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<DelpinIdentityContext>()
                .AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
