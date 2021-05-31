using Delpin.Application.Interfaces;
using Delpin.Domain.Entities.Identity;
using Delpin.Infrastructure.Data;
using Delpin.Infrastructure.Data.Repositories;
using Delpin.Infrastructure.Identity;
using Delpin.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Delpin.Infrastructure
{

    public static class InfrastructureServiceExtensions
    {
        // extend the Service so that additional "extension" methods can be written and used
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

            // Sets token parameters as extension to service 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSettings:Key"))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // sets extension Authorization for roles 
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsAdmin",
                    policy => policy.AddRequirements(
                        new ClaimsAuthorizationRequirement(ClaimTypes.Role, new[] { "Admin" })));

                opt.AddPolicy("IsSuperUser",
                    policy => policy.AddRequirements(
                        new ClaimsAuthorizationRequirement(ClaimTypes.Role, new[] { "SuperUser", "Admin" })));
            });

            services.AddScoped<TokenService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}