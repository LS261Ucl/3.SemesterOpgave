using Delpin.Application.Contracts.v1.ProductCategories;
using Microsoft.Extensions.DependencyInjection;

namespace Delpin.Application
{
    // extend the Service so that additional "extension" methods can be written and used
    public static class ApplicationServiceExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}