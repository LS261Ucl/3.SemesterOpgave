using Delpin.Mvc.Authorization;
using Delpin.Mvc.Services;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Delpin.Mvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllersWithViews(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddScoped<HttpClient>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddSingleton<ImageConverter>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.LoginPath = "/Account/Login";
                opt.ExpireTimeSpan = TimeSpan.FromDays(5);
                opt.LogoutPath = "/Account/Logout";
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsAdmin", policy => policy.Requirements.Add(new IsInRoleRequirement("Admin")));
                opt.AddPolicy("IsSuperUser", policy => policy.Requirements.Add(new IsInRoleRequirement("SuperUser")));
            });

            services.AddSingleton<IAuthorizationHandler, IsInRoleHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.User.HasClaim(x => x.Type == "Token") && context.Request.Path != "/account/login")
                    context.Response.Redirect("/account/login");

                await next.Invoke();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
