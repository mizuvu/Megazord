global using Microsoft.EntityFrameworkCore;
global using Zord.Core.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Zord.EntityFrameworkCore;

public static class ConfigureService
{
    /// <summary>
    /// Use default Repositories & Unit Of Work
    ///     Entities & DbContext will inject manualy
    /// </summary>
    public static IServiceCollection AddZordDynamicRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        return services;
    }

    /// <summary>
    /// Only use Unit Of Work, repository will auto inject with default
    /// </summary>
    public static IServiceCollection AddZordUnitOfWork<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<TContext>));

        return services;
    }
}