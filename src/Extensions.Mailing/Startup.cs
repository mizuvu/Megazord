using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System;
using Zord.Core.Mailing;

namespace Zord.Extensions.Mailing
{
    public static class Startup
    {
        public static IServiceCollection AddSmtpMail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("SMTPMail"));

            var useMailKit = configuration.GetValue<bool>("SMTPMail:PreferMailKit");

            if (useMailKit)
            {
                services.AddScoped<ISmtpMailService, SmtpMailKitService>();
            }
            else
            {
                services.AddScoped<ISmtpMailService, SmtpMailService>();
            }

            return services;
        }

        public static IServiceCollection AddGraphMail(this IServiceCollection services, Action<GraphOptions> action)
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