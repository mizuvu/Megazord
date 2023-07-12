using Microsoft.Extensions.DependencyInjection;
using Zord.DomainActiveDirectory.Interfaces;
using Zord.DomainActiveDirectory.Options;
using Zord.DomainActiveDirectory.Services;

namespace Zord.DomainActiveDirectory;

public static class Startup
{
    public static IServiceCollection AddDomainActiveDirectory(this IServiceCollection services, Action<DomainOptions>? action = null)
    {
        if (action == null)
        {
            services.AddTransient<IActiveDirectoryService, FakeActiveDirectoryService>();
        }
        else
        {
            services.Configure(action);
#pragma warning disable CA1416
            services.AddTransient<IActiveDirectoryService, ActiveDirectoryService>();
#pragma warning restore CA1416
        }

        return services;
    }

    public static IServiceCollection AddLdapActiveDirectory(this IServiceCollection services, Action<LdapOptions>? action = null)
    {
        if (action == null)
        {
            services.AddTransient<IActiveDirectoryService, FakeActiveDirectoryService>();
        }
        else
        {
            services.Configure(action);
#pragma warning disable CA1416
            services.AddTransient<IActiveDirectoryService, LDAPService>();
#pragma warning restore CA1416
        }

        return services;
    }
}