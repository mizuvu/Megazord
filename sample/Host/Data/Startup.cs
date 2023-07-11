using Host.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Zord.Core.Repositories;
using Zord.EntityFrameworkCore;
using Zord.EntityFrameworkCore.Cache;

namespace Host.Data;

public static class Startup
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services
                .AddDbContext<AlphaDbContext>(opt =>
                    opt.UseInMemoryDatabase("MemoryDb"));
        }
        else
        {
            services
                .AddDbContext<AlphaDbContext>(opt =>
                    opt.UseSqlServer(defaultConnection));
        }

        services.AddScoped(typeof(IRepository<>), typeof(CustomRepository<>));

        //services.AddZordDynamicRepositories();
        services.AddZordDynamicCacheRepositories();

        services.AddZordUnitOfWork<AlphaDbContext>();

        //services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));
        //services.AddScoped(typeof(IAppUnitOfWork), typeof(AppUnitOfWork));

        return services;
    }
}