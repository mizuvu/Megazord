namespace Host.Models;

public class ApiResult : Zord.Result.Result
{
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
}
