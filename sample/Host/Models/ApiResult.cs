namespace Host.Models;

public class ApiResult : Zord.Models.Result
{
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
}
