using Delpin.Application.Contracts.v1.ProductCategories;
using Microsoft.Extensions.DependencyInjection;

namespace Delpin.Application
{
    public static class ApplicationServiceExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}