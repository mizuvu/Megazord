using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Zord.Extensions.DependencyInjection
{
    public static class AutoAddServices
    {
        public static IServiceCollection AddFreeDependencies(this IServiceCollection services)
            => services
            .AddServices(typeof(ITransientDependency), ServiceLifetime.Transient)
            .AddServices(typeof(IScopedDependency), ServiceLifetime.Scoped)
            .AddServices(typeof(ISingletonDependency), ServiceLifetime.Singleton);

        private static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
        {
            var interfaceTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(t => interfaceType.IsAssignableFrom(t)
                                && t.IsClass && !t.IsAbstract)
                    .Select(t => new
                    {
                        Service = t.GetInterfaces().FirstOrDefault(),
                        Implementation = t
                    })
                    .Where(t => t.Service != null && interfaceType.IsAssignableFrom(t.Service));

            foreach (var type in interfaceTypes)
            {
                services.AddService(type.Service, type.Implementation, lifetime);
            }

            return services;
        }

        private static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceType, implementationType);
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentException("Invalid lifeTime", nameof(lifetime));
            }

            return services;
        }
    }
}

