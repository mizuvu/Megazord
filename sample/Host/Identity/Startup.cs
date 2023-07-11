using Microsoft.Extensions.Options;
using Zord.DomainActiveDirectory;
using Zord.DomainActiveDirectory.Configurations;
using Zord.Identity.EntityFrameworkCore;
using Zord.Identity.EntityFrameworkCore.Options;

namespace Host.Identity;

public static class Startup
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityData<AppIdentityDbContext>(configuration);

        services.AddHttpContextAccessor();

        services.AddIdentitySetup<AppIdentityDbContext>();

        services.AddClaimProvider<CustomApplicationClaim>();

        //var jwt = configuration.GetSection("JWT").Get<JwtConfiguration>();
        //services.AddJwtConfiguration(jwt);

        services.AddTransient<IConfigureOptions<JwtOptions>, CustomJwtOptions>();

        var domain = configuration.GetSection("ActiveDirectory").Get<ActiveDirectoryConfiguration>();
        services.AddDomainActiveDirectory(domain);

        services.AddIdentityServices();

        services.AddTest(opt =>
        {
            opt.Name = "Test";
            opt.Description = "Test description";
        });

        return services;
    }

    public static IServiceCollection AddTest(this IServiceCollection services, Action<TestOptions> action)
    {
        //services.Configure(action);

        //services.AddTransient<IConfigureOptions<TestOptions>, CustomTestOptions>();

        return services;
    }
}