﻿global using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Zord.EntityFrameworkCore.Cache;

namespace Zord.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddZordDynamicCacheRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICacheRepository<,>), typeof(CacheRepository<,>));

        return services;
    }
}