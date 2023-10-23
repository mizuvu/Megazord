using System.Text.Json;

namespace Zord.Api.Extensions
{
    public class LowercaseJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ConvertName();
        }
    }
}
