using Delpin.Application.Contracts.v1.Identity;
using Delpin.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Delpin.API.Test.IntegrationTest
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        protected const string BaseUrl = "https://localhost:5001/api/v1/";
        private readonly IServiceProvider _serviceProvider;

        // Creates WebAppFactory where it gets the DbContext service at runtime and replaces it with InMemoryDb
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var databaseDescriptor = services.SingleOrDefault(x =>
                        x.ServiceType == typeof(DbContextOptions<DelpinContext>));

                    if (databaseDescriptor != null)
                        services.Remove(databaseDescriptor);

                    services.AddDbContext<DelpinContext>(opt =>
                        opt.UseInMemoryDatabase("TestDb"));
                });
            });

            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        // Gets Jwt Token and sets it to the AuthenticationHeaderValue
        protected async Task AuthenticateAsync(string email = "mikkel@delpin.dk")
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await LoginAndGetJwtAsync(email));
        }

        // Logs in user and returns Jwt token
        private async Task<string> LoginAndGetJwtAsync(string email)
        {
            var response = await TestClient.PostAsJsonAsync(BaseUrl + "account/login",
                new LoginDto { Email = email, Password = "Pa$$w0rd" });

            var loginResponse = await response.Content.ReadFromJsonAsync<UserDto>();

            return loginResponse?.Token;
        }

        protected JsonSerializerOptions ReferenceHandlerOptions =>
            new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve };

        // Disposes of InMemory database after each test has run
        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var databaseContext = serviceScope.ServiceProvider.GetService<DelpinContext>();

            databaseContext?.Database.EnsureDeleted();

            GC.SuppressFinalize(this);
        }
    }
}
