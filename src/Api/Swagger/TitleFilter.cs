using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zord.Api.Swagger
{
    public class TitleFilter : IDocumentFilter
    {
        private readonly IConfiguration _configuration;

        public TitleFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            var title = _configuration.GetValue<string>("Swagger:ApiName");

            doc.Info.Title = title;
        }
    }
}
