using Microsoft.Extensions.DependencyInjection;

namespace Zord.EntityFrameworkCore.Dapper;

public static class Startup
{
    /// <summary>
    /// Inject this DI to use Dapper with DbContext
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDapper(this IServiceCollection services)
    {
        // use for run SQL raw
        services.AddScoped(typeof(IDapper<>), typeof(Dapper<>));

        return services;
    }
}