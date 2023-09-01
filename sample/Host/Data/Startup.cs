﻿using Host.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Zord.Extensions.DependencyInjection;

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

        services.AddZordUnitOfWork();

        //services.AddZordDynamicCacheRepositories();
        services.AddScoped(typeof(ICacheRepository<,>), typeof(CustomCacheRepository<,>));

        services.AddZordUnitOfWork<AlphaDbContext>();

        //services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));
        //services.AddScoped(typeof(IAppUnitOfWork), typeof(AppUnitOfWork));

        return services;
    }
}