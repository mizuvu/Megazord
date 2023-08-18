namespace Zord.Extensions.Logging;

public class MSSQLOptions
{
    public string Connection { get; set; } = default!;

    public string? TableName { get; set; }
}
