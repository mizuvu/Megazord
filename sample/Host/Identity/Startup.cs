using Zord.DomainActiveDirectory;
using Zord.Identity.EntityFrameworkCore;
using Zord.Identity.EntityFrameworkCore.Options;

namespace Host.Identity;

public static class Startup
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStr = configuration.GetConnectionString("DefaultConnection");

        services.AddIdentityData<AppIdentityDbContext>(connectionStr);

        services.AddHttpContextAccessor();

        services.AddIdentitySetup<AppIdentityDbContext>();

        //var jwt = configuration.GetSection("JWT").Get<JwtConfiguration>();
        //services.AddJwtConfiguration(jwt);

        services.AddOptions<JwtOptions>().BindConfiguration("JWT");

        services.AddDomainActiveDirectory(opt => opt.Name = "domain.com");

        services.AddIdentityServices();

        return services;
    }
}