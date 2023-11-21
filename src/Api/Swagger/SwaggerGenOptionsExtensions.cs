using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zord.Api.Swagger
{
    internal static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Common SwaggerGen Options configure
        /// </summary>
        internal static SwaggerGenOptions AddJwtSecurityScheme(this SwaggerGenOptions swaggerGenOptions)
        {
            // Include 'SecurityScheme' to use JWT Authentication
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            swaggerGenOptions.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });

            // fix Swagger when contain multi model, dto has same name
            swaggerGenOptions.CustomSchemaIds(x => x.FullName);

            return swaggerGenOptions;
        }
    }
}
