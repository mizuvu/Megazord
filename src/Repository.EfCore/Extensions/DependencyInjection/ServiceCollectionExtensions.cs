﻿global using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Zord.Repository;
using Zord.Repository.EntityFrameworkCore;

namespace Zord.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Use default Repositories & Unit Of Work
    ///     Entities & DbContext will inject manualy
    /// </summary>
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        return services;
    }

    /// <summary>
    /// Only use Unit Of Work, repository will auto inject with default
    /// </summary>
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<TContext>));

        return services;
    }

    /// <summary>
    /// Use custom Unit Of Work implement from base UOW abtractions
    /// </summary>
    public static IServiceCollection AddUnitOfWork<TInterface, TImplement>(this IServiceCollection services)
        where TInterface : IUnitOfWork
        where TImplement : class, TInterface
    {
        services.AddScoped(typeof(TInterface), typeof(TImplement));

        return services;
    }
}