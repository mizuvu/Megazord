using Microsoft.Extensions.DependencyInjection;
using Zord.Files;

namespace Zord.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFiles(this IServiceCollection services)
        {
            services.AddTransient<IExcelService, ExcelService>();

            return services;
        }
    }
}