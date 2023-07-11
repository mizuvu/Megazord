using Host.Identity.Jwt;
using Microsoft.Extensions.Options;
using Zord.Identity.EntityFrameworkCore.Options;

namespace Host.Identity
{
    public class CustomJwtOptions : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;

        public CustomJwtOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            var jwt = _configuration.GetSection("JWT").Get<JwtConfiguration>();

            ArgumentNullException.ThrowIfNull(jwt, nameof(JwtConfiguration));

            options.Issuer = jwt.TokenIssuer;
            options.SecretKey = jwt.SecretKey;
        }
    }
}
