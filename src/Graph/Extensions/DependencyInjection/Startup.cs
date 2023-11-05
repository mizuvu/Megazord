using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System;
using Zord.Graph;

namespace Zord.Extensions.DependencyInjection
{
    public static class Startup
    {
        public static IServiceCollection AddMicrosoftGraph(this IServiceCollection services, Action<GraphOptions> action)
        {
            // bind action to options
            var options = new GraphOptions();
            action.Invoke(options);

            var credential = new ClientSecretCredential(
                options.TenantId, options.ClientId, options.ClientSecret,
                new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud });

            //you can use a single client instance for the lifetime of the application
            services.AddSingleton(sp =>
            {
                return new GraphServiceClient(credential);
            });

            services.AddScoped<IGraphMailService, GraphMailService>();

            return services;
        }
    }
}