namespace Host.Identity
{
    public class GetTokenRequest
    {
        public GetTokenRequest(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
