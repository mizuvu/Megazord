using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zord.Identity.Migrator.MSSQL;

namespace Zord.Identity.Migrator.Extensions
{
    public static class ServiceCollections
    {
        /// <summary>
        ///     Register DI when run console.
        /// </summary>
        public static IServiceCollection AddServiceCollections(this IServiceCollection services, IConfiguration config)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            services.AddIdentityData<AppIdentityDbContext>(config, assemblyName);

            services.AddIdentitySetup<AppIdentityDbContext>();

            // manual inject services here
            services.AddScoped<AppDbContextInitialiser>();

            return services;
        }
    }
}
