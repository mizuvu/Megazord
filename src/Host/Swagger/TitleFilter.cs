using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zord.Host.Swagger
{
    public class TitleFilter(IConfiguration configuration) : IDocumentFilter
    {
        private readonly IConfiguration _configuration = configuration;

        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            var title = _configuration.GetValue<string>("Swagger:ApiName");

            doc.Info.Title = title;
        }
    }
}
