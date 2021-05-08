using System.Security.Claims;
using System.Text;
using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
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

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("MustBeAdmin",
                    policy => policy.AddRequirements(
                        new ClaimsAuthorizationRequirement(ClaimTypes.Role, new[] { "Admin" })));
                
                opt.AddPolicy("MustBeSuperUser",
                    policy => policy.AddRequirements(
                        new ClaimsAuthorizationRequirement(ClaimTypes.Role, new[] { "SuperUser", "Admin" })));
            });

            services.AddScoped<TokenService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}