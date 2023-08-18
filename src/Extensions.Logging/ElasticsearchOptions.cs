namespace Zord.Extensions.Logging;

public class ElasticsearchOptions
{
    public string? ServiceName { get; set; }

    public string Endpoint { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;
}
