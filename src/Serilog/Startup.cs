using Microsoft.Extensions.Hosting;
using Serilog;

namespace Zord.Serilog
{
    public static class Startup
    {
        public static IHostBuilder UseSerilog(this IHostBuilder host)
        {
            return host.UseSerilog(SerilogExtensions.Configure);
        }
    }
}