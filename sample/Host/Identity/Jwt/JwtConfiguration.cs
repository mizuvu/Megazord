namespace Host.Identity.Jwt;

public class JwtConfiguration
{
    public int RefreshTokenExpireInSeconds { get; set; }

    public string SecretKey { get; set; } = default!;

    public int TokenExpireInSeconds { get; set; }

    public string TokenIssuer { get; set; } = default!;
}