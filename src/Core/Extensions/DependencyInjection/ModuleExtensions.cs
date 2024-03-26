using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zord.Modules;

namespace Zord.Extensions.DependencyInjection;

public static class ModuleExtensions
{
    /// <summary>
    /// Scan & configure all module services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AutoConfigureModuleServices(this IServiceCollection services, IConfiguration configuration)
    {
        // get all classes inherit from interface
        var moduleServices = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x =>
                typeof(IModuleServiceCollection).IsAssignableFrom(x)
                && x.IsClass && !x.IsAbstract)
            .Select(s => Activator.CreateInstance(s) as IModuleServiceCollection);

        foreach (var instance in moduleServices)
        {
            instance?.ConfigureServices(services, configuration);
        }

        return services;
    }

    /// <summary>
    /// Scan & configure all module pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IApplicationBuilder AutoConfigureModulePipelines(this IApplicationBuilder builder, IConfiguration configuration)
    {
        // get all classes inherit from interface
        var modulePipelines = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x =>
                typeof(IModuleApplicationBuilder).IsAssignableFrom(x)
                && x.IsClass && !x.IsAbstract)
            .Select(s => Activator.CreateInstance(s) as IModuleApplicationBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.ConfigurePipelines(builder, configuration);
        }

        return builder;
    }
}
