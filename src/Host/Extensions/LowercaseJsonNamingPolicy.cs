using System.Text.Json;

namespace Zord.Host.Extensions
{
    public class LowercaseJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ConvertName();
        }
    }
}
