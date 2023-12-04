using System.Collections.Generic;

namespace Zord.Serilog
{
    public class WriteToOptions
    {
        public string Name { get; set; } = null!;

        public Dictionary<string, string>? Args { get; set; }
    }
}
