using Microsoft.Extensions.DependencyInjection;
using Zord.Extensions.Excel;

namespace Zord.Extensions.Documents
{
    public static class Startup
    {
        public static IServiceCollection AddZordDocumentGenerator(this IServiceCollection services)
        {
            services.AddTransient<IExcelService, ExcelService>();

            return services;
        }
    }
}