using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Zord.Api.Swagger;

namespace Zord.Api.Swagger;

public static class Startup
{
    private const string _sectionName = "Swagger";

    private static SwaggerSettings GetSettings(IConfiguration configuration)
    {
        var settings = configuration.GetSection(_sectionName).Get<SwaggerSettings>();

        ArgumentNullException.ThrowIfNull(settings, nameof(SwaggerSettings));

        return settings;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SwaggerSettings>(configuration.GetSection(_sectionName));

        var settings = GetSettings(configuration);

        if (settings.Enable)
        {
            services.AddSwaggerGen(opt => opt.CustomSchemaIds(x => x.FullName));
        }

        return services;
    }

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = GetSettings(configuration);

        if (settings.Enable)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                if (!string.IsNullOrEmpty(settings.Title))
                {
                    options.DocumentTitle = settings.Title;
                }
            });
        }

        return app;
    }

    public static IServiceCollection AddSwaggerWithVersion(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SwaggerSettings>(configuration.GetSection(_sectionName));

        var settings = GetSettings(configuration);

        if (settings.Enable)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, CustomSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.CustomSchemaIds(x => x.FullName);
            });
        }

        return services;
    }

    public static IApplicationBuilder UseSwaggerWithVersion(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = GetSettings(configuration);

        if (settings.Enable)
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                if (!string.IsNullOrEmpty(settings.Title))
                {
                    options.DocumentTitle = settings.Title;
                }

                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options
                        .SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                }
            });
        }

        return app;
    }
}