using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;
using Zord.DomainActiveDirectory.Configurations;
using Zord.DomainActiveDirectory.Interfaces;
using Zord.DomainActiveDirectory.Services;

namespace Zord.DomainActiveDirectory;

public static class Startup
{
    public static IServiceCollection AddDomainActiveDirectory(this IServiceCollection services, ActiveDirectoryConfiguration? configuration)
    {
        if (configuration == null)
        {
            services.AddTransient<IActiveDirectoryService, FakeActiveDirectoryService>();
        }
        else
        {
#pragma warning disable CA1416
            services.AddActiveDirectory(configuration);
#pragma warning restore CA1416
        }

        return services;
    }

    [SupportedOSPlatform("windows")]
    private static IServiceCollection AddActiveDirectory(this IServiceCollection services, ActiveDirectoryConfiguration configuration)
    {
        if (configuration.LDAP != null)
        {
            services.AddSingleton(configuration.LDAP);

            services.AddTransient<IActiveDirectoryService, LDAPService>();
        }
        else
        {
            services.AddSingleton(configuration);

            services.AddTransient<IActiveDirectoryService, ActiveDirectoryService>();
        }

        return services;
    }
}