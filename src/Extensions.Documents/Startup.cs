using Microsoft.Extensions.DependencyInjection;
using Zord.Core.Documents;

namespace Extensions.Documents
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