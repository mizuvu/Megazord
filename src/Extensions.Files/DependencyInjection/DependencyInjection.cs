using Microsoft.Extensions.DependencyInjection;
using Zord.Extensions.Files;

namespace Zord.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddZordDocuments(this IServiceCollection services)
        {
            services.AddTransient<IExcelService, ExcelService>();

            return services;
        }
    }
}