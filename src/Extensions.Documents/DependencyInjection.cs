using Microsoft.Extensions.DependencyInjection;
using Zord.Core;
using Zord.Extensions.Documents;

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