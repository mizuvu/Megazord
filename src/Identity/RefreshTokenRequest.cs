namespace Zord.Identity
{
    public class RefreshTokenRequest
    {
        public RefreshTokenRequest(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}