namespace Zord.Identity
{
    public class GetTokenRequest
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}
